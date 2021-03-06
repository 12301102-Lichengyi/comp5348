﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bank.Business.Components.Interfaces;
using Bank.Business.Entities;
using System.Transactions;


namespace Bank.Business.Components
{
    public class TransferProvider : ITransferProvider
    {
        /**
         * This function transfer "pAmount" from "pFromAcct" to "pToAcct"
         */
        public void Transfer(decimal pAmount, int pFromAcctNumber, int pToAcctNumber, String pDescription, String pReturnAddress)
        {
            OperationOutcome.OperationOutcomeResult lResult = OperationOutcome.OperationOutcomeResult.Successful;
            

            String lMessage = "TransferSuccessful";
            try
            {
                using (TransactionScope lScope = new TransactionScope())
                using (BankEntityModelContainer lContainer = new BankEntityModelContainer())
                {
                    Account lFromAcct = GetAccountFromNumber(pFromAcctNumber);
                    Account lToAcct = GetAccountFromNumber(pToAcctNumber);
                    
                    lFromAcct.Withdraw(pAmount);
                    lToAcct.Deposit(pAmount);

                    lContainer.Attach(lFromAcct);
                    lContainer.Attach(lToAcct);
                    lContainer.ObjectStateManager.ChangeObjectState(lFromAcct, System.Data.EntityState.Modified);
                    lContainer.ObjectStateManager.ChangeObjectState(lToAcct, System.Data.EntityState.Modified);
                    lContainer.SaveChanges();

                    lScope.Complete();
                }
            }
            catch (Exception lException)
            {
                Console.WriteLine("Error occured while transferring money:  " + lException.Message);
                
                lMessage = lException.Message;
                lResult = OperationOutcome.OperationOutcomeResult.Failure;
                //throw;
            }
            finally
            {
                /**
                 * notify if the transfer is successful 
                 */
                IOperationOutcomeService lService = OperationOutcomeServiceFactory.GetOperationOutcomeService(pReturnAddress);
                //Console.WriteLine("GUID - Bank:  " + pDescription);

                // USE THE RESPONSE QUEUE
                lService.NotifyOperationOutcome(new OperationOutcome() { Message = lMessage, Outcome = lResult, OperationReference = pDescription });

                //here you should know if the outcome of the transfer was successful or not
            }

        }

        private Account GetAccountFromNumber(int pToAcctNumber)
        {
            using (BankEntityModelContainer lContainer = new BankEntityModelContainer())
            {
                return lContainer.Accounts.Where((pAcct) => (pAcct.AccountNumber == pToAcctNumber)).FirstOrDefault();
            }
        }
    }
}
