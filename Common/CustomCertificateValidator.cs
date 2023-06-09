﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class CustomCertificateValidator : X509CertificateValidator
    {
        public override void Validate(X509Certificate2 certificate)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            if (!certificate.Verify())
            {
                throw new SecurityTokenValidationException("Certificate validation failed");
            }

            X509Store trustedPeopleStore = new X509Store(StoreName.TrustedPeople, StoreLocation.LocalMachine);

            try
            {
                trustedPeopleStore.Open(OpenFlags.ReadOnly);

                if (!trustedPeopleStore.Certificates.Contains(certificate))
                {
                    throw new SecurityTokenValidationException("Certificate is not in the Trusted People store");
                }
            }
            finally
            {
                trustedPeopleStore.Close();
            }
        }
    }
}
