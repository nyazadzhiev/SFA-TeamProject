using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Entities;

namespace WorkforceManagementAPI.BLL.Contracts
{
    public interface IValidationService
    {
        void EnsureUserExist(User user);

        void EnsureLenghtIsValid(string forCheck, int length, string message);

        void EnsureEmailIsValid(string email);

        Task EnsureEmailIsUniqueAsync(string email);

        void EnsureTeamExist(Team team);

        void CheckTeamName(string title);

        void EnsureTimeOffExist(TimeOff timeOff);

        Task EnsureUpdateEmailIsUniqueAsync(string email, User user);

        void EnsureInputFitsBoundaries(int input, int minValue, int maxValue);

        void ValidateDateRange(DateTime minValue, DateTime maxValue);

        void EnsureInputFitsBoundaries(DateTime input, DateTime minValue, DateTime maxValue);
    }
}
