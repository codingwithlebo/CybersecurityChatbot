namespace CybersecurityChatbot
{
    
    
        public static class ResponseHandler
        {
            private static readonly string[][] Keywords = new string[][]
            {
            new string[] { "how are you", "how r you" },
            new string[] { "hello", "hi", "hey" },
            new string[] { "purpose", "what can you do", "what can i ask" },
            new string[] { "password", "passwords" },
            new string[] { "phishing", "scam", "suspicious email" },
            new string[] { "browsing", "website", "https" },
            new string[] { "2fa", "two factor", "authentication" },
            new string[] { "malware", "virus", "ransomware" },
            new string[] { "exit", "quit", "bye" }
            };

            private static readonly string[] Responses = new string[]
            {
            "I am running at full capacity and ready to help you stay secure online!",
            "Hello! Ask me anything about staying safe online.",
            "I can help you with password safety, phishing, safe browsing, 2FA, and malware. Just ask!",
            "Use passwords that are at least 12 characters long, mixing letters, numbers and symbols. Never reuse passwords — use a password manager like Bitwarden instead.",
            "Phishing emails fake urgency like your account will be closed. Always check the senders real email address and never click suspicious links.",
            "Look for HTTPS and a padlock before entering personal info. Avoid public Wi-Fi for banking — use a VPN if you must.",
            "Enable two-factor authentication on every account that supports it. An authenticator app is safer than SMS codes.",
            "Keep your OS and apps updated — most malware exploits known vulnerabilities. Avoid downloading files from untrusted sources.",
            "QUIT"
            };

            public static string GetResponse(string input)
            {
                string lower = input.ToLower().Trim();

                for (int i = 0; i < Keywords.Length; i++)
                {
                    foreach (string keyword in Keywords[i])
                    {
                        if (lower.Contains(keyword))
                            return Responses[i];
                    }
                }

                return "I did not quite understand that. Could you rephrase? Try asking about passwords, phishing, safe browsing, 2FA, or malware.";
            }
        }
    }
