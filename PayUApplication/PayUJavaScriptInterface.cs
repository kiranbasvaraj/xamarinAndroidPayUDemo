using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Java.Interop;

namespace PayUApplication
{
    public class PayUJavaScriptInterface : Java.Lang.Object
    {
        Context mContext;
        public PayUJavaScriptInterface(Context c)
        {
            mContext = c;
        }
        // public void Success  
        [Export]
        [JavascriptInterface]
        public void success(long id, string paymentId)
        {
            Intent intent = new Intent(mContext, typeof(SuccessActivity));
            mContext.StartActivity(intent);
        }
        [Export]
        [JavascriptInterface]
        public void failure(long id, string paymentId)
        {
            Intent intent = new Intent(mContext, typeof(FailureActivity));
            mContext.StartActivity(intent);
        }
    }
}