using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace DC
{
    /// <summary>
    /// 模拟unity协程
    /// @author hannibal
    /// </summary>
    public class Coroutine
    {
        private LinkedList<IEnumerator> coroutineList = new LinkedList<IEnumerator>();

        public void StartCoroutine(IEnumerator ie)
        {
            coroutineList.AddLast(ie);
        }

        public void StopCoroutine(IEnumerator ie)
        {
            try
            {
                coroutineList.Remove(ie);
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
        }

        public void Tick()
        {
            var node = coroutineList.First;
            while (node != null)
            {
                IEnumerator ie = node.Value;
                bool ret = true;
                if (ie.Current is CustomYieldInstruction)
                {
                    CustomYieldInstruction wait = (CustomYieldInstruction)ie.Current;
                    //检测等待条件，条件满足，跳到迭代器的下一元素 （IEnumerator方法里的下一个yield）
                    if (wait.Tick())
                    {
                        ret = ie.MoveNext();
                    }
                }
                else
                {
                    ret = ie.MoveNext();
                }
                //迭代器没有下一个元素了，删除迭代器（IEnumerator方法执行结束）
                if (!ret)
                {
                    coroutineList.Remove(node);
                }
                //下一个迭代器
                node = node.Next;
            }
        }
    }
    /// <summary>
    /// 等待接口
    /// </summary>
    public interface CustomYieldInstruction
    {
        /// <summary>
        /// 每帧检测是否等待结束
        /// </summary>
        /// <returns></returns>
        bool Tick();
    }
    /// <summary>
    /// 按秒等待
    /// </summary>
    public class WaitForSeconds : CustomYieldInstruction
    {
        private float _seconds = 0f;
        private Stopwatch _stopwatch = new Stopwatch();

        public WaitForSeconds(float seconds)
        {
            _seconds = seconds;
            _stopwatch.Start();
        }

        public bool Tick()
        {
            float deltaTime = _stopwatch.ElapsedMilliseconds;
            return _seconds <= deltaTime * 0.001f;
        }
    }
    /// <summary>
    /// 按帧等待
    /// </summary>
    public class WaitForFrames : CustomYieldInstruction
    {
        private int _frames = 0;
        public WaitForFrames(int frames)
        {
            _frames = frames;
        }

        public bool Tick()
        {
            _frames -= 1;
            return _frames <= 0;
        }
    }
    /// <summary>
    /// 等待条件成立执行下一个
    /// </summary>
    public class WaitUntil : CustomYieldInstruction
    {
        private Func<bool> _func;
        public WaitUntil(Func<bool> func)
        {
            _func = func;
        }

        public bool Tick()
        {
            if (_func == null)
                return false;
            return _func.Invoke();
        }
    }
    /// <summary>
    /// 等待条件不成立执行下一个
    /// </summary>
    public class WaitWhile : CustomYieldInstruction
    {
        private Func<bool> _func;
        public WaitWhile(Func<bool> func)
        {
            _func = func;
        }

        public bool Tick()
        {
            if (_func == null)
                return false;
            return !_func.Invoke();
        }
    }
}
