using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using kursach.Model;
using kursach.Database;
using kursach.Services;
using System.Net;
using System.Net.Mail;
using System;
using kursach.View;

namespace kursach.ViewModel
{
    public class AdminViewModel : ViewModelBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserService _userService;
        private readonly EmailService _emailService;

        
        public ObservableCollection<UserRequestFull> UserRequests { get; set; }

        private UserRequestFull _selectedRequest;
        public UserRequestFull SelectedRequest
        {
            get => _selectedRequest;
            set
            {
                _selectedRequest = value;
                RaisePropertyChanged(nameof(SelectedRequest));
            }
        }

        public ICommand ApproveRequestCommand { get; }
        public ICommand RejectRequestCommand { get; }


        public ICommand OpenAddCardWindowCommand { get; }
        public AdminViewModel(AppDbContext appDbContext, UserService userService)
        {
            _appDbContext = appDbContext;
            _userService = userService;
            _emailService = new EmailService();

            LoadRequests();

            ApproveRequestCommand = new RelayCommand(ApproveRequest);
            RejectRequestCommand = new RelayCommand(RejectRequest);
            OpenAddCardWindowCommand = new RelayCommand(OpenAddCardWindow);
        }

        private void OpenAddCardWindow()
        {
            var addCardWindow = new AddCardWindow();
            addCardWindow.ShowDialog();
        }
        private void LoadRequests()
        {
            var requests = from request in _appDbContext.UserRequests
                           join card in _appDbContext.CardInformations
                           on request.CardId equals card.Id
                           select new UserRequestFull
                           {
                               Id = request.Id,
                               CardId = card.Id,
                               UserId = request.UserId,
                               UserFullName = request.UserFullName,
                               UserEmail = request.UserEmail,
                               UserBirthDate = request.UserBirthDate,
                               RentalTime = request.RentalTime,
                               Status = request.Status,

                               Card_Title = card.Title,
                               Card_Description = card.Description,
                               Card_ImageUrl = card.ImageUrl,
                               Card_Rental_Period = card.Rental_Period,
                               Card_Price = card.Price,
                               Card_Location = card.Location
                           };

            UserRequests = new ObservableCollection<UserRequestFull>(requests.ToList());
        }

        private void ApproveRequest()
        {
            if (SelectedRequest == null)
            {
                MessageBox.Show("Выберите заявку для одобрения.");
                return;
            }

            var requestToApprove = _appDbContext.UserRequests.FirstOrDefault(r => r.Id == SelectedRequest.Id);
            if (requestToApprove != null)
            {
                requestToApprove.Status = true; 
                _appDbContext.SaveChanges();

                SendApprovalEmail(requestToApprove.UserEmail, requestToApprove.UserFullName);

                UserRequests.Remove(SelectedRequest);

                MessageBox.Show("Заявка одобрена. Письмо отправлено пользователю.");
            }
        }


        private void RejectRequest()
        {
            if (SelectedRequest == null)
            {
                MessageBox.Show("Выберите заявку для отказа.");
                return;
            }

            var requestToDelete = _appDbContext.UserRequests.FirstOrDefault(r => r.Id == SelectedRequest.Id);

            if (requestToDelete != null)
            {
                _appDbContext.UserRequests.Remove(requestToDelete);
                _appDbContext.SaveChanges();
            }

            UserRequests.Remove(SelectedRequest);

            MessageBox.Show("Заявка удалена.");
        }


        private void SendApprovalEmail(string recipientEmail, string userName)
        {
            try
            {
                _emailService.Send(recipientEmail, "Молодец!", $"<h1>{recipientEmail} </h1>");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отправке письма: {ex.Message}");
            }
        }


    }
}
