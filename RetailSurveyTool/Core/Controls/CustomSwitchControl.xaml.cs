using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;




namespace Jci.RetailSurveyTool.TechnicianApp.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomSwitchControl : ContentView
    {
        public static BindableProperty IsEnableProperty =
           BindableProperty.Create(nameof(IsEnable), typeof(bool), typeof(CustomSwitchControl), defaultValue: false, defaultBindingMode: BindingMode.TwoWay);


        public bool IsEnable
        {
            get
            {
                return (bool)GetValue(IsEnableProperty);
            }
            set
            {
                SetValue(IsEnableProperty, value);
            }
        }

        bool selection1Active = true;
        bool selection2Active = false;
        private double valueX, valueY;
        private bool IsTurnX, IsTurnY;
        public CustomSwitchControl()
        {
            InitializeComponent();
            parentView.BindingContext = this;
            if (IsEnable)
            {
                //make selection2 move to selection 1
                selection1Active = false;
                selection2Active = true;
                text2.TextColor = Color.FromHex("#f7faf8");
                text1.TextColor = Color.FromHex("#141414");
                runningFrame.TranslateTo(runningFrame.X + 70, 0, 120);
                runningFrame.BackgroundColor = Color.FromHex("#04b34a");
            }
            else
            {
                selection1Active = true;
                text2.TextColor = Color.FromHex("#141414");
                text1.TextColor = Color.FromHex("#f7faf8");
                selection2Active = false;
                runningFrame.TranslateTo(runningFrame.X, 0, 120);
                runningFrame.BackgroundColor = Color.FromHex("#e30510");
                //var duration = TimeSpan.FromSeconds(1);
            }
        }
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == IsEnableProperty.PropertyName)
            {
                if (IsEnable)
                {
                    //make selection2 move to selection 1
                    selection1Active = false;
                    selection2Active = true;
                    text2.TextColor = Color.FromHex("#f7faf8");
                    text1.TextColor = Color.FromHex("#141414");
                    runningFrame.TranslateTo(runningFrame.X + 70, 0, 120);
                    runningFrame.BackgroundColor = Color.FromHex("#07a82c");    
                }
                else
                {
                    selection1Active = true;
                    text2.TextColor = Color.FromHex("#141414");
                    text1.TextColor = Color.FromHex("#f7faf8");
                    selection2Active = false;
                    runningFrame.TranslateTo(runningFrame.X, 0, 120);
                    runningFrame.BackgroundColor = Color.FromHex("#e30510");
                    //var duration = TimeSpan.FromSeconds(1);
                }
            }
        }

        public void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            var x = e.TotalX; // TotalX Left/Right
            var y = e.TotalY; // TotalY Up/Down

            // StatusType
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    Debug.WriteLine("Started");
                    break;
                case GestureStatus.Running:
                    Debug.WriteLine("Running");

                    // Check that the movement is x or y
                    if ((x >= 5 || x <= -5) && !IsTurnX && !IsTurnY)
                    {
                        IsTurnX = true;
                    }

                    if ((y >= 5 || y <= -5) && !IsTurnY && !IsTurnX)
                    {
                        IsTurnY = true;
                    }

                    // X (Horizontal)
                    if (IsTurnX && !IsTurnY)
                    {
                        if (x <= valueX)
                        {
                            // Left
                            if (selection2Active)
                            {
                                //make selection1 move to selection 2

                                selection1Active = true;
                                selection2Active = false;

                                text2.TextColor = Color.FromHex("#141414");
                                text1.TextColor = Color.FromHex("#f7faf8");
                                runningFrame.TranslateTo(runningFrame.X, 0, 120);
                                var duration = TimeSpan.FromSeconds(1);
                                //Vibration.Vibrate(duration);

                            }
                        }

                        if (x >= valueX)
                        {
                            // Right
                            if (selection1Active)
                            {
                                //make selection2 move to selection 1
                                selection1Active = false;
                                selection2Active = true;
                                text2.TextColor = Color.FromHex("#f7faf8");
                                text1.TextColor = Color.FromHex("#141414");
                                runningFrame.TranslateTo(runningFrame.X + 70, 0, 120);
                                var duration = TimeSpan.FromSeconds(1);
                                // Vibration.Vibrate(duration);

                            }
                        }
                    }


                    break;
                case GestureStatus.Completed:
                    Debug.WriteLine("Completed");

                    valueX = x;
                    valueY = y;

                    IsTurnX = false;
                    IsTurnY = false;

                    break;
                case GestureStatus.Canceled:
                    Debug.WriteLine("Canceled");
                    break;


            }
        }

        void OnText1Tapped(System.Object sender, System.EventArgs e)
        {
            if (selection2Active)
            {
                IsEnable = false;
            }
        }

        void OnText2Tapped(System.Object sender, System.EventArgs e)
        {
            if (selection1Active)
            {
                IsEnable = true;
            }
        }
    }
}