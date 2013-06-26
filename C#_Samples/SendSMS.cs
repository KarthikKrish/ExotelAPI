/*
 * This is a sample code that you can use to Send 
 * SMS-es using your Exotel Account.
 * Caution:
 * 1) This is the author's first ever C# code
 * 2) This was tested on linux mono
 * 
 * How to run this standalone on a linux box
 * 1) gmcs SendSMS.cs -r:System.Web.dll
 * 2) mono SendSMS.exe
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
	public class SendSMS
	{
		private string SID = null;
		private string token = null;

		public SendSMS (string SID, string token)
		{
			this.SID = SID;
			this.token = token;
		}

		public string execute (string from, string to, string Body)
		{
			Dictionary<string, string> postValues = new Dictionary<string, string> ();
			postValues.Add ("From", from);
			postValues.Add ("To", to);
			postValues.Add ("Body", Body);

			String postString = "";

			foreach (KeyValuePair<string, string> postValue in postValues) {
				postString += postValue.Key + "=" + HttpUtility.UrlEncode (postValue.Value) + "&";
			}
			postString = postString.TrimEnd ('&');
			/*
		 * Allow self signed certificates and such
		 */
			ServicePointManager.ServerCertificateValidationCallback = delegate {
				return true;
			};
			string smsURL = "https://twilix.exotel.in/v1/Accounts/<Your Exotel Sid>/Sms/send";
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
			//SendSMS s = new SendSMS ("YourExotelSID", "YourExotelToken");
			//string response = s.execute ("Your Exotel VN", "Customer's Phone no", "Message to send");
			//Console.WriteLine (response);
		}
	}
}

