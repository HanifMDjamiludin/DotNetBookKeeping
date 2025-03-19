namespace WebApplication1.Models;
public class User
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }    

    public List<Account> Accounts { get; set;} = new List<Account>();

}

public class BusinessUser : User
{
    public int calculateTax()
    {
        return 1;
    }
    
}


public class Account 
{
    public int AccountID {get; set;}
    public string AccountName { get; set; }
    public decimal Balance { get; set; }

    public List<Transaction> Transactions { get; set; } = new List<Transaction>();

    public void addTransaction(Transaction transaction){

        Transactions.Add(transaction);
        Balance += transaction.Amount;

    }
}

public class Transaction 
{
    public int TransactionID { get; set; }
    public DateTime Date { get; set; }

    public string Description { get; set; }
    public decimal Amount { get; set; }

    public int AccountID { get; set; }

    public Transaction(DateTime date, string description, decimal amount, int accountId){
      Date = date;
      Description = description;
      Amount = amount;
      AccountID = accountId;
    }
}