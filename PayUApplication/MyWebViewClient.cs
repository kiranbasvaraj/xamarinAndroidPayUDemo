using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net.Http;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;

namespace PayUApplication
{
    public class MyWebViewClient : WebViewClient
    {
        public override void OnPageStarted(WebView view, string url, Android.Graphics.Bitmap favicon)
        {
            base.OnPageStarted(view, url, favicon);
        }
        public override void OnPageFinished(WebView view, string url)
        {
            base.OnPageFinished(view, url);
        }
        public override void OnReceivedSslError(WebView view, SslErrorHandler handler, SslError error)
        {
            Log.Info("Error", "Exception caught!");
            handler.Cancel();
        }
    }
}