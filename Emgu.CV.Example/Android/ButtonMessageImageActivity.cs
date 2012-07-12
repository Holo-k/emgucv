//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

using System.Threading;
using Emgu.CV;
using Emgu.CV.Structure;

namespace AndroidExamples
{
   public class ButtonMessageImageActivity : Activity
   {
      private Button _clickButton;
      private TextView _messageText;
      private ImageView _imageView;
      private String _buttonText;
      private ProgressDialog _progress;

      public ButtonMessageImageActivity(String buttonText)
         : base()
      {
         _buttonText = buttonText;
         //dummy code to load the opencv libraries
         CvInvoke.CV_FOURCC('m', 'j', 'p', 'g');
      }

      protected override void OnCreate(Bundle savedInstanceState)
      {
         base.OnCreate(savedInstanceState);

         _clickButton = new Button(this);
         _clickButton.Text = _buttonText;
         _clickButton.LayoutParameters = new ViewGroup.LayoutParams(
            ViewGroup.LayoutParams.FillParent,
            ViewGroup.LayoutParams.WrapContent);

         _messageText = new TextView(this);
         _messageText.SetTextAppearance(this, Android.Resource.Attribute.TextAppearanceSmall);
         _imageView = new ImageView(this);
         _imageView.LayoutParameters = new ViewGroup.LayoutParams(
            ViewGroup.LayoutParams.FillParent,
            ViewGroup.LayoutParams.MatchParent);

         LinearLayout layout = new LinearLayout(this);
         layout.Orientation = Orientation.Vertical;
         layout.LayoutParameters = new ViewGroup.LayoutParams(
            ViewGroup.LayoutParams.FillParent,
            ViewGroup.LayoutParams.FillParent);

         layout.AddView(_clickButton);
         layout.AddView(_messageText);
         layout.AddView(_imageView);

         ScrollView scrollView = new ScrollView(this);
         scrollView.AddView(layout);
         SetContentView(scrollView);

         _progress = new ProgressDialog(this) { Indeterminate = true };
         _progress.SetTitle("Processing");
         _progress.SetMessage("Please wait...");

         _clickButton.Click += delegate(Object sender, EventArgs e)
         {
            if (OnButtonClick != null)
            {
               _progress.Show();

               ThreadPool.QueueUserWorkItem(delegate
               {
                  try
                  {
                     OnButtonClick(sender, e);
                  }
                  finally
                  {
                     RunOnUiThread(() => { _progress.Hide(); });
                  }
               }
               );
            }
         };
      }

      public EventHandler<EventArgs> OnButtonClick;

      public void SetMessage(String message)
      {
         RunOnUiThread(() => { _messageText.Text = message; });
      }

      public void SetImageBitmap(Bitmap image)
      {
         RunOnUiThread(() => { _imageView.SetImageBitmap(image); });
      }
   }
}