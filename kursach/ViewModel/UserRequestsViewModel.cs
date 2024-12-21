using kursach.Model;
using kursach.Database;
using kursach.Services;
using kursach.View.Pages;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class UserRequestsViewModel : INotifyPropertyChanged
{
    private readonly AppDbContext _appDbContext;
    private readonly UserService _userService;

    public ObservableCollection<UserRequestFull> UserRequestsFull { get; set; }


    public void LoadData()
    {
        // Загрузка заявок пользователя с обновлённым статусом
        var data = _appDbContext.UserRequests
            .Where(userRequest => userRequest.UserId == _userService.CurrentUserId)
            .Join(
                _appDbContext.CardInformations,
                userRequest => userRequest.CardId,
                cardInfo => cardInfo.Id,
                (userRequest, cardInfo) => new UserRequestFull
                {
                    Id = userRequest.Id,
                    CardId = userRequest.CardId,
                    UserId = userRequest.UserId,
                    UserEmail = userRequest.UserEmail,
                    UserFullName = userRequest.UserFullName,
                    UserBirthDate = userRequest.UserBirthDate,
                    RentalTime = userRequest.RentalTime,
                    Status = userRequest.Status,
                    Card_Title = cardInfo.Title,
                    Card_Description = cardInfo.Description,
                    Card_ImageUrl = cardInfo.ImageUrl,
                    Card_Rental_Period = cardInfo.Rental_Period,
                    Card_Price = cardInfo.Price,
                    Card_Location = cardInfo.Location
                })
            .ToList();

        // Обновляем ObservableCollection
        if (UserRequestsFull == null)
            UserRequestsFull = new ObservableCollection<UserRequestFull>(data);
        else
        {
            UserRequestsFull.Clear();
            foreach (var item in data)
                UserRequestsFull.Add(item);
        }

  ;
    }

    public UserRequestsViewModel(AppDbContext appDbContext, UserService userService)
    {
        _appDbContext = appDbContext;
        _userService = userService;

        LoadData();

      
    }

    public void RefreshData()
    {
        LoadData();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
