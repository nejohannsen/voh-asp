using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace voh_asp.Services
{
    public interface IAWSSecrets
    {
        string AWSAccessKeyId { get; }
        string AWSSecretAccessKey { get; }
    }
    public class AWSSecrets : IAWSSecrets
    {
        private readonly string _AWSAccessKeyId;
        private readonly string _AWSSecretAccessKey;

        public AWSSecrets(string AWSAccessKeyId, string AWSSecretAccessKey)
        {
            _AWSAccessKeyId = AWSAccessKeyId;
            _AWSSecretAccessKey = AWSSecretAccessKey;
        }

        public string AWSAccessKeyId { get { return _AWSAccessKeyId; } }
        public string AWSSecretAccessKey { get { return _AWSSecretAccessKey; } }
    }
}
