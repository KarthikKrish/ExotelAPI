/*
 * This is a sample code that you can use to make calls
 * using your Exotel account.
 * Caution:
 * 1) This is the author's second ever C# code
 * 2) This was tested on linux mono
 * 
 * How to run this standalone on a linux box
 * 1) gmcs ConnectCall.cs -r:System.Web.dll
 * 2) mono ConnectCall.exe
 * 
 */
using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Collections.Specialized;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace ExotelSDK
{
	public class ConnectCall
	{
		private string SID = null;
		private string token = null;

		public ConnectCall (string SID, string token)
		{
			this.SID = SID;
			this.token = token;
		}

		public string connectCustomerToAgent (string from, string to, string callerID,
		                                      string callType, string timeLimit = null,
		                                      string timeOut = null, string statusCallback = null)
		{
			Dictionary<string, string> postValues = new Dictionary<string, string> ();
			postValues.Add ("From", from);
			postValues.Add ("To", to);
			postValues.Add ("CallerID", callerID);
			postValues.Add ("CallType", callType);
			if (timeLimit != null) {
				postValues.Add ("TimeLimit", timeLimit);
			}
			if (timeOut != null) {
				postValues.Add ("TimeOut", timeOut);
			}

			if (statusCallback != null) {
				postValues.Add ("StatusCallback", statusCallback);
			}

			String postString = "";

			foreach (KeyValuePair<string, string> postValue in postValues) {
				postString += postValue.Key + "=" + HttpUtility.UrlEncode (postValue.Value) + "&";
			}
			postString = postString.TrimEnd ('&');

			return(this.sendRequest (postString));

		}

		public string connectCustomerToApp (string from, string url, string callerID,
		                                     string callType, string timeLimit = null,
		                                     string timeOut = null, string statusCallback = null,
		                                     string customfield = null)
		{
			Dictionary<string, string> postValues = new Dictionary<string, string> ();
			postValues.Add ("From", from);
			postValues.Add ("Url", url);
			postValues.Add ("CallerID", callerID);
			postValues.Add ("CallType", callType);
			if (timeLimit != null) {
				postValues.Add ("TimeLimit", timeLimit);
			}
			if (timeOut != null) {
				postValues.Add ("TimeOut", timeOut);
			}

			if (statusCallback != null) {
				postValues.Add ("StatusCallback", statusCallback);
			}
			if (customfield != null) {
				postValues.Add ("CustomField", customfield);
			}

			String postString = "";

			foreach (KeyValuePair<string, string> postValue in postValues) {
				postString += postValue.Key + "=" + HttpUtility.UrlEncode (postValue.Value) + "&";
			}
			postString = postString.TrimEnd ('&');

			return(this.sendRequest (postString));
		}

		private string sendRequest (string postString)
		{
			/*
		 	* Allow self signed certificates and such
			*/
			ServicePointManager.ServerCertificateValidationCallback = delegate {
				return true;
			};
			string smsURL = "https://twilix.exotel.in/v1/Accounts/Exotel/Calls/connect";
			HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create (smsURL);
			objRequest.Credentials = new NetworkCredential (this.SID, this.token);
			objRequest.Method = "POST";
			objRequest.ContentLength = postString.Length;
			objRequest.ContentType = "application/x-www-form-urlencoded";
			// post data is sent as a stream                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
			StreamWriter opWriter = null;
			opWriter = new StreamWriter (objRequest.GetRequestStream ());
			opWriter.Write (postString);
			opWriter.Close ();

			// returned values are returned as a stream, then read into a string                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            
			HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse ();
			string postResponse = null;
			using (StreamReader responseStream = new StreamReader(objResponse.GetResponseStream())) {
				postResponse = responseStream.ReadToEnd ();
				responseStream.Close ();
			}

			return (postResponse);
		}
		public static void Main (string[] args)
		{
	//		ConnectCall c = new ConnectCall ("YourExotelSID", "YourExotelToken");
	//		string response = c.connectCustomerToAgent("Customer's no", "Agent's no", "Your Exotel VN", "trans");
	//		Console.WriteLine(response);
	//		string response = c.connectCustomerToApp("Customer's no", "http://my.exotel.in/exoml/start/<app id>","Your Exotel VN","trans");
	//		Console.WriteLine(response);
		}
	}
}

