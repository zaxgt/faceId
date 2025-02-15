using System;
using System.IO;
using System.Net.Http;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using System.Drawing;

class Program
{
    static async System.Threading.Tasks.Task Main()
    {
        string imageUrl = "put your link to face picture here contact zaxandmilo on discord for help"; 
        string cascadePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "haarcascade_frontalface_default.xml");

        // Load OpenCV Haar Cascade Classifier
        var faceCascade = new CascadeClassifier(cascadePath);
        var recognizer = new LBPHFaceRecognizer(1, 8, 8, 8, 123.0);

        // Download image from URL
        Mat knownFaceImage = await DownloadImageFromUrl(imageUrl);
        if (knownFaceImage == null)
        {
            Console.WriteLine("Failed to download or process the image.");
            return;
        }

        var grayFace = new Mat();
        CvInvoke.CvtColor(knownFaceImage, grayFace, ColorConversion.Bgr2Gray);

        // Detect the face in the image
        var faces = faceCascade.DetectMultiScale(grayFace, 1.1, 5, new Size(30, 30));
        foreach (var face in faces)
        {
            var faceRegion = new Mat(grayFace, face);
            recognizer.Train(new[] { faceRegion }, new[] { 0 }); // Train with label 0
        }

        // Start webcam
        var capture = new VideoCapture(0);

        while (true)
        {
            var frame = new Mat();
            capture.Read(frame);
            if (frame.IsEmpty)
                break;

            var gray = new Mat();
            CvInvoke.CvtColor(frame, gray, ColorConversion.Bgr2Gray);

            // Detect faces
            faces = faceCascade.DetectMultiScale(gray, 1.1, 5, new Size(30, 30));

            foreach (var face in faces)
            {
                var roiGray = new Mat(gray, face);
                var result = recognizer.Predict(roiGray);
                int label = result.Label;
                double confidence = result.Distance;

                // Check if prediction is valid
                if (label == 0 && confidence < 70) // Stricter confidence threshold
                {
                    Console.WriteLine("Face Verified. Access granted!");

                    string flagFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Obs_settings.txt");
                    File.WriteAllText(flagFilePath, "verified");

                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Face Denied. Access Denied!");

                    string flagFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Obs_settings.txt");
                    File.WriteAllText(flagFilePath, "Denied");
                }
            }

            if (CvInvoke.WaitKey(1) == 'q') // Press 'q' to exit manually
                break;
        }

        capture.Release();
        CvInvoke.DestroyAllWindows();
    }

    static async System.Threading.Tasks.Task<Mat> DownloadImageFromUrl(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                byte[] imageData = await client.GetByteArrayAsync(url);
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    Bitmap bitmap = new Bitmap(ms);
                    return bitmap.ToMat(); // Convert to OpenCV Mat
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error downloading image: " + ex.Message);
                return null;
            }
        }
    }
}
