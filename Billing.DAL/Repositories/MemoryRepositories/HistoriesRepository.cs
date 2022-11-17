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
            throw new KeyNotFoundException($"History key isn't found. Key is {history.Id}");

        User? user = context.Users.FirstOrDefault(x => x.Id == history.User.Id);
        if(user == null)
            throw new KeyNotFoundException($"User key isn't found. Key is {history.User.Id}");
        
        Coin? coin = context.Coins.FirstOrDefault(x => x.Id == history.Coin.Id);
        if(coin == null)
            throw new KeyNotFoundException($"Coin key isn't found. Key is {history.Coin.Id}");
        
        History newHistory = new History(user, coin) { Id = history.Id };
        context.Histories.Remove(oldHistory);
        context.Histories.Add(newHistory);
    }

    public async Task<History> Get(long id)
    {
        History? history = context.Histories.FirstOrDefault(x => x.Id == id);
        if(history == null)
            throw new KeyNotFoundException($"History key isn't found. Key is {id}");

        return await GetNewHistory(history);
    }

    public async Task<IEnumerable<History>> Get()
    {
        IEnumerable<History> histories = await Task.WhenAll(context.Histories.Select(async x => await GetNewHistory(x)));
        return histories;
    }

    private async Task<History> GetNewHistory(History history)
    {
        return await new Task<History>(() => 
        {
            User newUser = new User(history.User.Name, history.User.Rating) { Id = history.User.Id };
            User coinUser = new User(history.Coin.User.Name, history.Coin.User.Rating) 
            {
                Id = history.Coin.User.Id
            };
            Coin newCoin = new Coin(coinUser){ Id = history.Coin.Id };
            return new History(newUser, newCoin) { Id = history.Id };
        });
    }
}
