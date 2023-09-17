## 主要功能  
    用C#实现的Coroutine，使用方式类似于Unity的Coroutine。核心原理是利用了C#提供的迭代器  
## 协程类
	CustomYieldInstruction：接口类，需要继承它
	WaitForSeconds：按秒等待
	WaitForFrames：按帧数等待
	WaitUntil：等待条件成立，跳到下一个迭代器
	WaitWhile：与WaitUntil相反，等待条件不成立时跳到下一个迭代器
## 使用方式举例
    class Client
    {
        private static Coroutine _coroutine;
        private static int _currFrame = 0;
        static void Main(string[] args)
        {
            _coroutine = new Coroutine();
            _coroutine.StartCoroutine(WaitForFrames());
            _coroutine.StartCoroutine(WaitForSeconds());
            _coroutine.StartCoroutine(WaitUntil());
            _coroutine.StartCoroutine(WaitWhile());
            _coroutine.StartCoroutine(YieldBreak());
            _coroutine.StartCoroutine(YieldReturnNull());

            while (true)
            {
                _currFrame++;
                _coroutine.Tick();
                Thread.Sleep(1);
            }
        }
        private static IEnumerator WaitForFrames()
        {
            Console.WriteLine("WaitForFrames:" + _currFrame);
            yield return new WaitForFrames(3);
            Console.WriteLine("WaitForFrames:" + _currFrame);
            yield return new WaitForFrames(3);
            Console.WriteLine("WaitForFrames:" + _currFrame);
        }
        private static IEnumerator WaitForSeconds()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.WriteLine("WaitForSeconds:" + stopwatch.ElapsedMilliseconds * 0.001f);
            yield return new WaitForSeconds(3);
            Console.WriteLine("WaitForSeconds:" + stopwatch.ElapsedMilliseconds * 0.001f);
            yield return new WaitForSeconds(3);
            Console.WriteLine("WaitForSeconds:" + stopwatch.ElapsedMilliseconds * 0.001f);
        }
        private static IEnumerator WaitUntil()
        {
            Console.WriteLine("WaitUntil:" + _currFrame);
            yield return new WaitUntil(()=> _currFrame == 10);
            Console.WriteLine("WaitUntil:" + _currFrame);
        }
        private static IEnumerator WaitWhile()
        {
            Console.WriteLine("WaitWhile:" + _currFrame);
            yield return new WaitWhile(() => _currFrame < 10);
            Console.WriteLine("WaitWhile:" + _currFrame);
        }
        private static IEnumerator YieldBreak()
        {
            Console.WriteLine("Start YieldBreak");
            yield break;
            Console.WriteLine("End YieldBreak");
        }
        private static IEnumerator YieldReturnNull()
        {
            Console.WriteLine("YieldReturnNull:" + _currFrame);
            yield return null;
            Console.WriteLine("YieldReturnNull:" + _currFrame);
            yield return null;
            Console.WriteLine("YieldReturnNull:" + _currFrame);
        }
    }
