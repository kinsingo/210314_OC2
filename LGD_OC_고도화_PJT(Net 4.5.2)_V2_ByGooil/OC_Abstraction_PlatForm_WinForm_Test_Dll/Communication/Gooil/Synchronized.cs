using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OC_Abstraction_PlatForm_WinForm_Test_Dll.Communication.Gooil
{
    public interface SingleTon
    {
        void Release();
    }

    public abstract class SynchronizedSingleTon<T> : SingleTon where T : class, new()
    {
        private static Lazy<T> mInstance = new Lazy<T>();
        private static Mutex mtx = new Mutex();
        protected SynchronizedSingleTon() { }

        public T getInstance()
        {
            return SynchronizedSingleTon<T>.GetInstance();
        }
        public void Release()
        {
            mInstance = null;
        }
        public static T GetInstance()
        {
            if (mInstance != null)
                return mInstance.Value;
            else
            {
                mtx.WaitOne();
                if (mInstance == null)
                {
                    mInstance = new Lazy<T>();
                }
                mtx.ReleaseMutex();

                return mInstance.Value;
            }
        }

        public static void ReleaseInstance()
        {
            mInstance = null;
        }


    }
}
