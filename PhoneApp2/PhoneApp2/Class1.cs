using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Linq;

namespace PhoneApp2
{
    class Demo 
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region private Helpers

        /// <summary>
        /// Raise the PropertyChanged event and pass along the property that changed
        /// </summary>
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        #endregion

        public Demo()
        {
            if (ResultList == null)
            {
                ResultList = new ObservableCollection<String>();
            }
            
        }

        // collection of forecasts for each time period
        public static ObservableCollection<String> ResultList
        {
            get;
            set;
        }


        public void Button_Click()
        {
            string avatarUri = "http://query.yahooapis.com/v1/public/yql?q=select * from geo.places where text=\"SFO\" ";
            //avatarUri = "http://query.yahooapis.com/v1/public/yql?q=" + "select%20*%20from%20yahoo.finance.quotes%20where%20symbol%20in%20({0})" + "&amp;env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";
            HttpWebRequest request =
                (HttpWebRequest)HttpWebRequest.Create(avatarUri);

            DemoState forecastState = new DemoState();
            forecastState.AsyncRequest = request;

            // start the asynchronous request
            request.BeginGetResponse(new AsyncCallback(DemoHandleResponse),
                forecastState);

            //MainPage.res = "success";
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
                Console.WriteLine(streamResult);
                XDocument doc = XDocument.Load(streamResult);
                //XElement root = XElement.Load(streamResult);
                //MainPage.res = "1";
                //ResultList.Add("1");
                XElement element = doc.Root.Element("results");
                if (element != null)
                {
                    //ResultList.Clear();
                    ResultList.Add("Success");
                }
                else
                {
                    //ResultList.Clear();
                    ResultList.Add("Fail");
                }
               
                /*foreach (XElement curElement in doc.Root.Elements("result"))
                {
                    ResultList.Add("1");
                }*/
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
