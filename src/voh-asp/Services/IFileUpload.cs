using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using voh_asp.Models;

namespace voh_asp.Services
{
    public interface IFileUpload
    {

        //Size GetSizeFromImage(string bucketName, string keyName);
        bool UploadFile(Stream File, AppFile dbFile, bool publicReadable = true);
        bool UploadFile(Stream File, string BucketName, string Filename, out string error, string Path = null, bool publicReadable = true);
        bool DeleteFile(AppFile file);
        string CreateURL(AppFile dbFile);
        string GetPresignedURL(string keyname, string bucket, int timeoutHours = 1, bool asAttachment = false);
        string GetSafeFileName(string filename);
        byte[] GetFileStream(string keyName, string bucketName);
    }
}



//    public class AWSRepository
//{
//    /// <summary>
//    /// Upload stream file of to specific Amazon Bucket with specified Filename, optional Path, and optional public readable flag
//    /// </summary>
//    /// <param name="File">Stream File to be uploaded</param>
//    /// <param name="BucketName">Name of Amazon Bucket to which the file should be uploaded</param>
//    /// <param name="Filename">Filename of the file</param>
//    /// <param name="Path">Path within the bucket where the file should reside (default is null)</param>
//    /// <param name="publicReadable">Should the file be publicly accessible (default is false)</param>
//    /// <returns>Boolean</returns>
//    public bool UploadFile(Stream File, string BucketName, string Filename, string Path = null, bool publicReadable = false)
//    {
//        bool success = true;

//        try
//        {
//            string keyName = Filename;

//            if (!string.IsNullOrWhiteSpace(Path))
//            {
//                keyName = Path + "/" + Filename;
//            }

//            TransferUtility fileTransferUtility =
//                 new TransferUtility(new AmazonS3Client(GlobalVariables.AWSRegionEndpoint));

//            TransferUtilityUploadRequest fileTransferUtilityRequest = new TransferUtilityUploadRequest
//            {
//                BucketName = BucketName,
//                InputStream = File,
//                StorageClass = S3StorageClass.Standard,
//                PartSize = 6291456,
//                Key = keyName,
//                ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256
//            };

//            if (publicReadable)
//            {
//                fileTransferUtilityRequest.CannedACL = S3CannedACL.PublicRead;
//            }

//            fileTransferUtility.Upload(fileTransferUtilityRequest);
//        }
//        catch (AmazonS3Exception)
//        {
//            success = false;
//        }
//        catch (Exception)
//        {
//            success = false;
//        }

//        return success;
//    }

//    public bool UploadFile(Stream File, string BucketName, string Filename, out string error, string Path = null, bool publicReadable = false)
//    {
//        bool success = true;

//        error = string.Empty;

//        try
//        {
//            string keyName = Filename;

//            if (!string.IsNullOrWhiteSpace(Path))
//            {
//                keyName = Path + "/" + Filename;
//            }

//            TransferUtility fileTransferUtility =
//                 new TransferUtility(new AmazonS3Client(GlobalVariables.AWSRegionEndpoint));

//            TransferUtilityUploadRequest fileTransferUtilityRequest = new TransferUtilityUploadRequest
//            {
//                BucketName = BucketName,
//                InputStream = File,
//                StorageClass = S3StorageClass.Standard,
//                PartSize = 6291456,
//                Key = keyName,
//                ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256
//            };

//            if (publicReadable)
//            {
//                fileTransferUtilityRequest.CannedACL = S3CannedACL.PublicRead;
//            }

//            fileTransferUtility.Upload(fileTransferUtilityRequest);
//        }
//        catch (AmazonS3Exception s3Exception)
//        {
//            error = "s3Exception ";
//            error += s3Exception.Message;
//            if (s3Exception.InnerException != null)
//            {
//                error += s3Exception.InnerException.Message;
//            }
//            success = false;
//        }
//        catch (System.Net.WebException ex)
//        {
//            error = "WebException ";
//            error += ex.Message + " ";
//            error += ex.Status + " ";
//            if (ex.InnerException != null)
//            {
//                error += ex.InnerException.Message;
//            }
//            success = false;
//        }
//        catch (Exception ex)
//        {
//            error = "Exception ";
//            error += ex.Message;
//            if (ex.InnerException != null)
//            {
//                error += ex.InnerException.Message;
//            }
//            success = false;
//        }

//        return success;
//    }

//    /// <summary>
//    /// Delete a file from Amazon AWS giving the Bucket name and keyname
//    /// </summary>
//    /// <param name="bucketName">Bucket name where the file resides</param>
//    /// <param name="keyName">Key name of the file to be deleted (includes path)</param>
//    /// <returns>Boolean</returns>
//    public bool DeleteFile(string bucketName, string keyName)
//    {
//        bool success = true;
//        IAmazonS3 client;
//        using (client = new AmazonS3Client(GlobalVariables.AWSRegionEndpoint))
//        {
//            DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest
//            {
//                BucketName = bucketName,
//                Key = keyName
//            };
//            try
//            {
//                client.DeleteObject(deleteObjectRequest);
//                success = true;

//            }
//            catch (AmazonS3Exception)
//            {
//                success = false;
//            }
//        }
//        return success;

//    }

//    /// <summary>
//    /// Creates a web accessible URL given an AWS Bucket name and File Keyname (assumes AWS Region set in Global Variables)
//    /// </summary>
//    /// <param name="AWSBucket">Name of Bucket</param>
//    /// <param name="keyName">File Key Name</param>
//    /// <returns>String</returns>
//    public string CreateURL(string AWSBucket, string keyName)
//    {
//        return "https://" + AWSBucket + ".s3-" + GlobalVariables.AWSRegion + ".amazonaws.com/" + keyName;
//    }

//    /// <summary>
//    /// Gets a presigned URL for accessing a secure Amazon AWS file with optional timeout in hours
//    /// </summary>
//    /// <param name="keyname">Key name of the file</param>
//    /// <param name="bucket">Bucket where the file resides</param>
//    /// <param name="timeoutHours">Optional timeout value in hours</param>
//    /// <param name="asAttachment">Optional should the url be set up to be downloaded</param>
//    /// <returns>String</returns>
//    public static string GetPresignedURL(string keyname, string bucket, int timeoutHours = 1, bool asAttachment = false)
//    {

//        using (AmazonS3Client client = new AmazonS3Client(GlobalVariables.AWSRegionEndpoint))
//        {
//            GetPreSignedUrlRequest request = new GetPreSignedUrlRequest()
//            {
//                BucketName = bucket,
//                Key = keyname,
//                Expires = DateTime.Now.AddHours(timeoutHours),
//            };

//            if (asAttachment)
//            {
//                request.ResponseHeaderOverrides.ContentDisposition = "attachment";
//            }

//            return client.GetPreSignedURL(request);
//        }
//    }

//    /// <summary>
//    /// Strips unsafe characters from a filename for use with Amazon S3 file storage. Allowed characters are a-z 0-9 _- and . (no spaces). 
//    /// </summary>
//    /// <param name="filename">Filename to be parsed</param>
//    /// <returns>String</returns>
//    public string GetSafeFileName(string filename)
//    {
//        System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9-.]");
//        return rgx.Replace(filename, "").ToLower();
//    }

//    public Size GetSizeFromImage(string bucketName, string keyName)
//    {
//        Size result = new Size();

//        using (AmazonS3Client client = new AmazonS3Client(GlobalVariables.AWSRegionEndpoint))
//        {
//            GetObjectRequest request = new GetObjectRequest
//            {
//                BucketName = bucketName,
//                Key = keyName
//            };

//            using (GetObjectResponse response = client.GetObject(request))
//            using (Stream responseStream = response.ResponseStream)
//            {
//                System.Drawing.Image image = System.Drawing.Image.FromStream(responseStream);

//                result.Height = image.Height;
//                result.Width = image.Width;
//            }
//        }

//        return result;
//    }

//    public static byte[] GetFileStream(string keyName, string bucketName)
//    {
//        using (AmazonS3Client client = new AmazonS3Client(GlobalVariables.AWSRegionEndpoint))
//        {
//            GetObjectRequest getObjectRequest = new GetObjectRequest()
//            {
//                BucketName = bucketName,
//                Key = keyName
//            };

//            using (GetObjectResponse getObjectResponse = client.GetObject(getObjectRequest))
//            {
//                using (Stream responseStream = getObjectResponse.ResponseStream)
//                {
//                    byte[] buffer = new byte[responseStream.Length];
//                    using (MemoryStream memStream = new MemoryStream())
//                    {
//                        int read;
//                        while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
//                        {
//                            memStream.Write(buffer, 0, read);
//                        }
//                        return memStream.ToArray();
//                    }
//                }
//            }
//        }
//    }


//}
