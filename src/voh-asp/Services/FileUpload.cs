using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using voh_asp.Models;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Extensions.Options;

namespace voh_asp.Services
{
    public class FileUpload : IFileUpload
    {

        private readonly IAWSSecrets _secrets;

        public FileUpload(IAWSSecrets secrets)
        {
            _secrets = secrets;
        }

        public string CreateURL(AppFile dbFile)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("https://s3-us-west-2.amazonaws.com");
            sb.Append("/");
            sb.Append(dbFile.AWSBucket);
            sb.Append("/");
            sb.Append(dbFile.Path);
            sb.Append("/");
            sb.Append(dbFile.AppFileId);
            sb.Append("/");
            sb.Append(dbFile.FileName);
            return sb.ToString();
        }

        public bool DeleteFile(AppFile file)
        {
            bool success = true;
            try
            {
                DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = file.AWSBucket,
                    Key = file.Path + "/" + file.AppFileId + "/" + file.FileName
                };
                //new AmazonS3Client(Environment.GetEnvironmentVariable("AWSAccessKeyId"),
                using (var client = new AmazonS3Client(Environment.GetEnvironmentVariable("AWSAccessKeyId"),
                                                            Environment.GetEnvironmentVariable("AWSSecretAccessKey"),
                                                            Amazon.RegionEndpoint.USWest2))
                {
                    client.DeleteObjectAsync(deleteObjectRequest);
                }
            }
            catch (AmazonS3Exception ex)
            {
                success = false;
            }
            catch (Exception ex)
            {
                success = false;
            }

            return success;
        }

        public byte[] GetFileStream(string keyName, string bucketName)
        {
            using (AmazonS3Client client = new AmazonS3Client(Environment.GetEnvironmentVariable("AWSAccessKeyId"),
                                                            Environment.GetEnvironmentVariable("AWSSecretAccessKey"),
                                                            Amazon.RegionEndpoint.USWest2))
            {
                GetObjectRequest getObjectRequest = new GetObjectRequest()
                {
                    BucketName = bucketName,
                    Key = keyName
                };

                using (GetObjectResponse getObjectResponse = client.GetObjectAsync(getObjectRequest).Result)
                {
                    using (Stream responseStream = getObjectResponse.ResponseStream)
                    {
                        byte[] buffer = new byte[responseStream.Length];
                        using (MemoryStream memStream = new MemoryStream())
                        {
                            int read;
                            while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                memStream.Write(buffer, 0, read);
                            }
                            return memStream.ToArray();
                        }
                    }
                }
            }
        }

        public string GetPresignedURL(string keyname, string bucket, int timeoutHours = 1, bool asAttachment = false)
        {
            throw new NotImplementedException();
        }

        public string GetSafeFileName(string filename)
        {
            throw new NotImplementedException();
        }

        //public Size GetSizeFromFile(string bucketName, string keyName)
        //{
        //    Size result = new Size();

        //    using (AmazonS3Client client = new AmazonS3Client(Amazon.RegionEndpoint.USWest2))
        //    {
        //        GetObjectRequest request = new GetObjectRequest
        //        {
        //            BucketName = bucketName,
        //            Key = keyName
        //        };

        //        using (GetObjectResponse response = client.GetObjectAsync(request).Result)
        //        using (Stream responseStream = response.ResponseStream)
        //        {
        //            System.Drawing. image = System.Drawing.Image.FromStream(responseStream);

        //            result.Height = image.Height;
        //            result.Width = image.Width;
        //        }
        //    }

        //    return result;
        //}

        public bool UploadFile(Stream File, AppFile dbFile, bool publicReadable = true)
        {
            bool success = true;

            try
            {
                string keyName = dbFile.Path + "/" + dbFile.AppFileId + "/" + dbFile.FileName;

                //AKIAJZNBCB7FAUTJJD7A
                TransferUtility fileTransferUtility =
                     new TransferUtility(new AmazonS3Client(_secrets.AWSAccessKeyId,
                                                            _secrets.AWSSecretAccessKey,
                                                            Amazon.RegionEndpoint.USWest2));

                TransferUtilityUploadRequest fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = dbFile.AWSBucket,
                    InputStream = File,
                    StorageClass = S3StorageClass.Standard,
                    PartSize = 6291456,
                    Key = keyName,
                    ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256
                };

                if (publicReadable)
                {
                    fileTransferUtilityRequest.CannedACL = S3CannedACL.PublicRead;
                }

                fileTransferUtility.Upload(fileTransferUtilityRequest);
            }
            catch (AmazonS3Exception ex)
            {
                success = false;
            }
            catch (Exception ex)
            {
                success = false;
            }

            return success;
        }

        public bool UploadFile(Stream File, string BucketName, string Filename, out string error, string Path = null, bool publicReadable = true)
        {
            throw new NotImplementedException();
        }
    }
}
