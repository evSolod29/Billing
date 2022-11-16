using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Billing.DAL.Contexts;
using Billing.DAL.Models;

namespace Billing.DAL.Repositories.MemoryRepositories;

public class HistoriesRepository
{
    private readonly MemoryContext context;

    public HistoriesRepository(MemoryContext context)
    {
        this.context = context;
    }

    public async Task Add(History history)
    {
        User? user = context.Users.FirstOrDefault(x => x.Id == history.User.Id);
        if(user == null)
            throw new KeyNotFoundException($"User key is {history.User.Id}");
        
        Coin? coin = context.Coins.FirstOrDefault(x => x.Id == history.Coin.Id);
        if(coin == null)
            throw new KeyNotFoundException($"Coin key is {history.Coin.Id}");
        
        History newHistory = new History(user, coin) { Id = context.Histories.Count + 1 };
        context.Histories.Add(newHistory);
        history.Id = newHistory.Id;
    }

    public async Task Update(History history)
    {
        History? oldHistory = context.Histories.FirstOrDefault(x => x.Id == history.Id);
        if(oldHistory == null)
            throw new KeyNotFoundException($"History key is {history.Id}");

        User? user = context.Users.FirstOrDefault(x => x.Id == history.User.Id);
        if(user == null)
            throw new KeyNotFoundException($"User key is {history.User.Id}");
        
        Coin? coin = context.Coins.FirstOrDefault(x => x.Id == history.Coin.Id);
        if(coin == null)
            throw new KeyNotFoundException($"Coin key is {history.Coin.Id}");
        
        History newHistory = new History(user, coin) { Id = history.Id };
        context.Histories.Remove(oldHistory);
        context.Histories.Add(newHistory);
    }
}
