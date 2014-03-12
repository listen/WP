using System;

namespace PhoneApp2
{
    public class Demo : INotifyPropertyChanged
    {
        public Demo()
        {
            ResultList = new ObservableCollection<String>();
        }

        // collection of forecasts for each time period
        public ObservableCollection<String> ResultList
        {
            get;
            set;
        }


        public void Button_Click(object sender, RoutedEventArgs e)
        {
            string avatarUri = "http://query.yahooapis.com/v1/public/yql?q=select * from geo.places where text=\"sunnyvale, ca\" ";
            HttpWebRequest request =
                (HttpWebRequest)HttpWebRequest.Create(avatarUri);

            DemoState forecastState = new DemoState();
            forecastState.AsyncRequest = request;

            // start the asynchronous request
            request.BeginGetResponse(new AsyncCallback(DemoHandleResponse),
                forecastState);

            //Button btn = (Button)sender;
            //btn.Content = this.list;
        }

        void DemoHandleResponse(IAsyncResult result)
        {
            // get the state information
            DemoState forecastState = (DemoState)result.AsyncState;
            HttpWebRequest forecastRequest = (HttpWebRequest)forecastState.AsyncRequest;

            // end the async request
            forecastState.AsyncResponse = (HttpWebResponse)forecastRequest.EndGetResponse(result);
            Stream streamResult;

            try
            {
                // get the stream containing the response from the async call
                streamResult = forecastState.AsyncResponse.GetResponseStream();
                XElement root = XElement.Load(streamResult);
                String res;
                foreach (XElement curElement in root.Elements("test"))
                {
                    ResultList.add(curElement.Name.ToString());
                }
            }
            catch (FormatException)
            {
                // there was some kind of error processing the response from the web
                // additional error handling would normally be added here
                return;
            }
        }

        public class DemoState
        {
            public HttpWebRequest AsyncRequest { get; set; }
            public HttpWebResponse AsyncResponse { get; set; }
        }

    }
}