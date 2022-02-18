namespace Enoch.CrossCutting.AwsS3
{
    public class AwsStorage
    {
        //    private readonly IAmazonS3 _s3Client;

        //    public AwsS3(IAmazonS3 s3Client)
        //    {
        //        _s3Client = s3Client;
        //    }

        //    public async Task<bool> UploadFileAsync(byte[] file, string keyName, string bucketName)
        //    {
        //        try
        //        {
        //            using (var fileTransferUtility = new TransferUtility(_s3Client))
        //            {
        //                using (var fileToUpload = new MemoryStream(file))
        //                {
        //                    await fileTransferUtility.UploadAsync(fileToUpload,
        //                        bucketName, keyName);
        //                }
        //            }

        //            return true;
        //        }
        //        catch (AmazonS3Exception)
        //        {
        //            return false;
        //        }
        //        catch (Exception)
        //        {
        //            return false;
        //        }
        //    }

        //    public string UrlFile(string keyName, string bucketName)
        //    {
        //        try
        //        {
        //            var request1 = new GetPreSignedUrlRequest
        //            {
        //                BucketName = bucketName,
        //                Key = keyName,
        //                Expires = DateTime.Now.AddMinutes(30)
        //            };
        //            return _s3Client.GetPreSignedURL(request1);
        //        }
        //        catch (AmazonS3Exception)
        //        {
        //            return null;
        //        }
        //        catch (Exception)
        //        {
        //            return null;
        //        }
        //    }

        //    public async Task<bool> Delete(string keyName, string bucketName)
        //    {
        //        try
        //        {
        //            var deleteObjectRequest = new DeleteObjectRequest
        //            {
        //                BucketName = bucketName,
        //                Key = keyName,
        //            };

        //            await _s3Client.DeleteObjectAsync(deleteObjectRequest);

        //            return true;
        //        }
        //        catch (AmazonS3Exception e)
        //        {
        //            return false;
        //        }
        //        catch (Exception e)
        //        {
        //            return false;
        //        }
        //    }
        //}

    }
}
