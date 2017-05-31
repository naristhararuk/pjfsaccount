using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Data.NHibernate.Dao;

using SS.Standard.Data.NHibernate.QueryCreator;

using System.Collections;
using SS.Standard.Security;

using SS.Standard.Utilities;
using System.Data;
using SS.DB.Query;

using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.Query;
using System.Security.Cryptography;


namespace SS.Standard.WorkFlow.Service.Implement
{
    public class WorkFlowsmsTokenService : ServiceBase<WorkFlowsmsToken, long>, IWorkFlowsmsTokenService
    {
        public IWorkFlowsmsTokenQuery WorkFlowsmsTokenQuery { get; set; }
        public IWorkFlowResponseTokenQuery WorkFlowResponseTokenQuery { get; set; }
        #region IWorkFlowsmsTokenService Members
        public override IDao<WorkFlowsmsToken, long> GetBaseDao()
        {
            return SS.Standard.WorkFlow.DAL.WorkFlowDaoProvider.WorkFlowsmsTokenDao;
        }
        public long GetRunning()
        {
            return GenerateRunning();
        }
        private long GenerateRunning()
        {
            long runningNumber = 0;
            try
            {
                IList<WorkFlowsmsToken> WorkFlowsmsTokenList = WorkFlowsmsTokenQuery.FindAll();
                if (WorkFlowsmsTokenList != null && WorkFlowsmsTokenList.Count > 0)
                {

                    runningNumber = WorkFlowsmsTokenList[0].Running;
                    if (runningNumber == 99999)
                    {
                        AddRunning(runningNumber);
                    }
                    else
                    {
                        WorkFlowsmsTokenList[0].Running++;
                        runningNumber = WorkFlowsmsTokenList[0].Running;
                        UpdateRunning(WorkFlowsmsTokenList[0]);
                    }
                }
                else
                {
                    //Initial Values 5 Digits
                    runningNumber++;
                    AddRunning(runningNumber);

                }
            }
            catch (Exception)
            {
                
                throw;
            }
            
            return runningNumber;
        }
        private void AddRunning(long RunningNumber)
        {
            try
            {
                WorkFlowsmsToken newRunning = new WorkFlowsmsToken();
                newRunning.ID = 1;
                newRunning.Running = RunningNumber;
                newRunning.CreDate = DateTime.Now;
                this.Save(newRunning);

            }
            catch (Exception)
            {
                
                throw;
            }

            
        }
        private void UpdateRunning(WorkFlowsmsToken updateRunning)
        {
            try
            {
                this.Update(updateRunning);

            }
            catch (Exception)
            {

                throw;
            }


        }

        public string GetSMSTokenCode(long workflowID)
        {
            string tokenCode = string.Empty;
            for (int i = 0; i < 99; i++)
            {
                tokenCode = SS.Standard.Utilities.Encryption.Md5Hash(DateTime.Now.ToString() + workflowID.ToString());
                //tokenCode = MakeReferencePattern(ReferenceAlphaCreator.Create(3), ReferenceNumericCreator.Create(2), PatternCreator.Create());
                if (IsDubplicateTokenCode(tokenCode))
                {
                    tokenCode = SS.Standard.Utilities.Encryption.Md5Hash(DateTime.Now.ToString() + workflowID.ToString());
                    //tokenCode = MakeReferencePattern(ReferenceAlphaCreator.Create(3), ReferenceNumericCreator.Create(2), PatternCreator.Create());
                }
                else
                {
                    break;
                }
 
            }
            return tokenCode;
        }

        private bool IsDubplicateTokenCode(string tokenCode)
        {
            IList<WorkFlowResponseToken> token = WorkFlowResponseTokenQuery.FindByTokenCode(tokenCode);
            if (token != null && token.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        #endregion

        #region Create Reference Number
        public static class PatternCreator
        {
            private static readonly char[] _passwordChars = "123456789".ToCharArray();

            private static readonly int _passwordCharLength = _passwordChars.Length;

            public static string Create()
            {
                int length = 1;
                // Get the seed.
                int seed = GetSeed();
                // Create the random class
                Random rnd = new Random(seed);
                // Create array for password.
                char[] password = new char[length];



                for (int i = 0; i < password.Length; i++)
                {
                    password[i] = _passwordChars[rnd.Next(0, _passwordCharLength - 1)];
                }
                return new string(password);
            }

            private static int GetSeed()
            {
                byte[] bytes = new byte[4];
                RandomNumberGenerator rng = RandomNumberGenerator.Create();
                rng.GetBytes(bytes);

                return
                    bytes[0] |
                    bytes[1] << 8 |
                    bytes[2] << 16 |
                    bytes[3] << 24;
            }
        }

        public static class ReferenceAlphaCreator
        {
            private static readonly char[] _passwordChars = "ABCDEFGHIJKLMNPQRSTUVWXYZ".ToCharArray();

            private static readonly int _passwordCharLength = _passwordChars.Length;

            public static string Create(int length)
            {
                // Get the seed.
                int seed = GetSeed();
                // Create the random class
                Random rnd = new Random(seed);
                // Create array for password.
                char[] password = new char[length];



                for (int i = 0; i < password.Length; i++)
                {
                    password[i] = _passwordChars[rnd.Next(0, _passwordCharLength - 1)];
                }
                return new string(password);
            }

            private static int GetSeed()
            {
                byte[] bytes = new byte[4];
                RandomNumberGenerator rng = RandomNumberGenerator.Create();
                rng.GetBytes(bytes);

                return
                    bytes[0] |
                    bytes[1] << 8 |
                    bytes[2] << 16 |
                    bytes[3] << 24;
            }
        }

        public static class ReferenceNumericCreator
        {
            private static readonly char[] _passwordChars = "123456789".ToCharArray();

            private static readonly int _passwordCharLength = _passwordChars.Length;

            public static string Create(int length)
            {
                // Get the seed.
                int seed = GetSeed();
                // Create the random class
                Random rnd = new Random(seed);
                // Create array for password.
                char[] password = new char[length];



                for (int i = 0; i < password.Length; i++)
                {
                    password[i] = _passwordChars[rnd.Next(0, _passwordCharLength - 1)];
                }
                return new string(password);
            }

            private static int GetSeed()
            {
                byte[] bytes = new byte[4];
                RandomNumberGenerator rng = RandomNumberGenerator.Create();
                rng.GetBytes(bytes);

                return
                    bytes[0] |
                    bytes[1] << 8 |
                    bytes[2] << 16 |
                    bytes[3] << 24;
            }
        }

       

        private string MakeReferencePattern(string valAlhpa, string ValNumeric, string Pattern)
        {
            string strResult = string.Empty;
            StringBuilder tempValue = new StringBuilder();
            char[] numeric = ValNumeric.ToCharArray();
            char[] alpha = valAlhpa.ToCharArray();
            int valPattern = int.Parse(Pattern);
            if (valPattern == 0)
            {
                //alpha   0,2,-,1,-
                //numeric -,-,1,-,0
                tempValue.Append(alpha.GetValue(0).ToString());
                tempValue.Append(alpha.GetValue(2).ToString());
                tempValue.Append(numeric.GetValue(1).ToString());
                tempValue.Append(alpha.GetValue(1).ToString());
                tempValue.Append(numeric.GetValue(0).ToString());
                strResult = tempValue.ToString();

            }
            else if (valPattern == 1)
            {
                //alpha   1,0,-,2,-
                //numeric -,-,1,-,0
                tempValue.Append(alpha.GetValue(1).ToString());
                tempValue.Append(alpha.GetValue(0).ToString());
                tempValue.Append(numeric.GetValue(1).ToString());
                tempValue.Append(alpha.GetValue(2).ToString());
                tempValue.Append(numeric.GetValue(0).ToString());
                strResult = tempValue.ToString();
            }
            else if (valPattern == 2)
            {
                //alpha   1,0,-,2,-
                //numeric -,-,0,-,1
                tempValue.Append(alpha.GetValue(1).ToString());
                tempValue.Append(alpha.GetValue(0).ToString());
                tempValue.Append(numeric.GetValue(0).ToString());
                tempValue.Append(alpha.GetValue(2).ToString());
                tempValue.Append(numeric.GetValue(1).ToString());
                strResult = tempValue.ToString();

            }
            else if (valPattern == 3)
            {
                //alpha   1,-,0,2,-
                //numeric -,0,-,-,1
                tempValue.Append(alpha.GetValue(1).ToString());
                tempValue.Append(numeric.GetValue(0).ToString());
                tempValue.Append(alpha.GetValue(0).ToString());
                tempValue.Append(alpha.GetValue(2).ToString());
                tempValue.Append(numeric.GetValue(1).ToString());
                strResult = tempValue.ToString();

            }
            else if (valPattern == 4)
            {
                //alpha   2,-,1,0,-
                //numeric -,0,-,-,1
                tempValue.Append(alpha.GetValue(2).ToString());
                tempValue.Append(numeric.GetValue(0).ToString());
                tempValue.Append(alpha.GetValue(1).ToString());
                tempValue.Append(alpha.GetValue(0).ToString());
                tempValue.Append(numeric.GetValue(1).ToString());
                strResult = tempValue.ToString();

            }
            else if (valPattern == 5)
            {
                //alpha   2,-,0,1,-
                //numeric -,0,-,-,1
                tempValue.Append(alpha.GetValue(2).ToString());
                tempValue.Append(numeric.GetValue(0).ToString());
                tempValue.Append(alpha.GetValue(0).ToString());
                tempValue.Append(alpha.GetValue(1).ToString());
                tempValue.Append(numeric.GetValue(1).ToString());
                strResult = tempValue.ToString();

            }
            else if (valPattern == 6)
            {
                //alpha   2,-,0,1,-
                //numeric -,1,-,-,0
                tempValue.Append(alpha.GetValue(2).ToString());
                tempValue.Append(numeric.GetValue(1).ToString());
                tempValue.Append(alpha.GetValue(0).ToString());
                tempValue.Append(alpha.GetValue(1).ToString());
                tempValue.Append(numeric.GetValue(0).ToString());
                strResult = tempValue.ToString();

            }
            else if (valPattern == 7)
            {
                //alpha   2,0,1,-,-
                //numeric -,-,-,1,0
                tempValue.Append(alpha.GetValue(2).ToString());
                tempValue.Append(alpha.GetValue(0).ToString());
                tempValue.Append(alpha.GetValue(1).ToString());
                tempValue.Append(numeric.GetValue(1).ToString());
                tempValue.Append(numeric.GetValue(0).ToString());
                strResult = tempValue.ToString();

            }
            else if (valPattern == 8)
            {
                //alpha   2,0,-,1,-
                //numeric -,-,1,-,0
                tempValue.Append(alpha.GetValue(2).ToString());
                tempValue.Append(alpha.GetValue(0).ToString());
                tempValue.Append(numeric.GetValue(1).ToString());
                tempValue.Append(alpha.GetValue(1).ToString());
                tempValue.Append(numeric.GetValue(0).ToString());
                strResult = tempValue.ToString();

            }
            else if (valPattern == 9)
            {
                //alpha   -,0,2,1,-
                //numeric 1,-,-,-,0
                tempValue.Append(numeric.GetValue(1).ToString());
                tempValue.Append(alpha.GetValue(0).ToString());
                tempValue.Append(alpha.GetValue(2).ToString());
                tempValue.Append(alpha.GetValue(1).ToString());
                tempValue.Append(numeric.GetValue(0).ToString());
                strResult = tempValue.ToString();

            }
            return strResult;
        }

        #endregion 

    }
}
