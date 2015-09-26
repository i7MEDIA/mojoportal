/// Author:				Christian Fredh
/// Created:			2007-04-29
/// Last Modified:		2015-03-13
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software. 

using System;
using System.Data;
using mojoPortal.Business;
using PollFeature.Data;
using System.Collections.Generic;

namespace PollFeature.Business
{
    /// <summary>
    /// Represents an instance of a poll
    /// </summary>
    public class Poll
    {
        private const string featureGuid = "0d0d0518-bfb6-480c-b34f-4d8c8e947f7c";

        public static Guid FeatureGuid
        {
            get { return new Guid(featureGuid); }
        }

        #region Constructors


        public Poll()
        { }

        public Poll(Guid pollGuid)
        {
            if (pollGuid != Guid.Empty)
            {
                GetPoll(pollGuid);
            }
        }

        public Poll(int moduleID)
        {
            if (moduleID >= 0)
            {
                GetPoll(moduleID);
            }
        }

        #endregion

        #region Private Properties

        private Guid pollGuid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private string question = String.Empty;
        private bool activated = true;
        private int totalVotes = 0;
        private bool anonymousVoting = false;
        private bool allowViewingResultsBeforeVoting = false;
        private bool showOrderNumbers = false;
        private bool showResultsWhenDeactivated = false;
        private DateTime activeFrom = DateTime.UtcNow;
        private DateTime activeTo = DateTime.UtcNow.AddYears(1);
        private Guid newPollGuid = Guid.NewGuid();

        #endregion

        #region Public Properties

        public Guid PollGuid
        {
            get { return pollGuid; }
        }

        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }

        public string Question
        {
            get { return question; }
            set { question = value; }
        }

        public bool IsActive
        {
            get
            {
                if (DateTime.UtcNow < activeFrom) return false;
                if (DateTime.UtcNow > activeTo) return false;
                return activated;
            }
        }

        public int TotalVotes
        {
            get { return totalVotes; }
        }

        public bool AnonymousVoting
        {
            get { return anonymousVoting; }
            set { anonymousVoting = value; }
        }

        public bool AllowViewingResultsBeforeVoting
        {
            get { return allowViewingResultsBeforeVoting; }
            set { allowViewingResultsBeforeVoting = value; }
        }

        public bool ShowOrderNumbers
        {
            get { return showOrderNumbers; }
            set { showOrderNumbers = value; }
        }

        public bool ShowResultsWhenDeactivated
        {
            get { return showResultsWhenDeactivated; }
            set { showResultsWhenDeactivated = value; }
        }

        public DateTime ActiveFrom
        {
            get { return activeFrom; }
            set { activeFrom = value; }
        }

        public DateTime ActiveTo
        {
            get { return activeTo; }
            set { activeTo = value; }
        }

        public bool Activated
        {
            get { return activated; }
            set { activated = value; }
        }

        #endregion


        #region Private Methods

        private void GetPoll(Guid pollGuid)
        {
            using (IDataReader reader = DBPoll.GetPoll(pollGuid))
            {
                GetPoll(reader);
            }
        }

        private void GetPoll(int moduleID)
        {
            using (IDataReader reader = DBPoll.GetPollByModuleID(moduleID))
            {
                GetPoll(reader);
            }
        }

        private void GetPoll(IDataReader reader)
        {
            if (reader.Read())
            {
                this.pollGuid = new Guid(reader["PollGuid"].ToString());
                this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                this.question = reader["Question"].ToString();
                this.activated = Convert.ToBoolean(reader["Active"]);
                this.anonymousVoting = Convert.ToBoolean(reader["AnonymousVoting"]);
                this.allowViewingResultsBeforeVoting = Convert.ToBoolean(reader["AllowViewingResultsBeforeVoting"]);
                this.showOrderNumbers = Convert.ToBoolean(reader["ShowOrderNumbers"]);
                this.showResultsWhenDeactivated = Convert.ToBoolean(reader["ShowResultsWhenDeactivated"]);
                this.activated = Convert.ToBoolean(reader["Active"]);
                this.activeFrom = Convert.ToDateTime(reader["ActiveFrom"]);
                this.activeTo = Convert.ToDateTime(reader["ActiveTo"]);
                try
                {
                    this.totalVotes = int.Parse(reader["TotalVotes"].ToString());
                }
                catch (FormatException) { }
            }
            
        }

        private bool Create()
        {
            pollGuid = newPollGuid;

            int rowsAffected = DBPoll.Add(
                this.pollGuid,
                this.siteGuid,
                this.question,
                this.anonymousVoting,
                this.allowViewingResultsBeforeVoting,
                this.showOrderNumbers,
                this.showResultsWhenDeactivated,
                this.activated,
                this.activeFrom,
                this.activeTo);

            return (rowsAffected > 0);
        }

        private bool Update()
        {
            return DBPoll.Update(
                this.pollGuid,
                this.question,
                this.anonymousVoting,
                this.allowViewingResultsBeforeVoting,
                this.showOrderNumbers,
                this.showResultsWhenDeactivated,
                this.activated,
                this.activeFrom,
                this.activeTo);
        }

        #endregion


        #region Public Methods

        public bool Save()
        {
            if (pollGuid == Guid.Empty) return Create();
            return Update();
        }

        public bool ClearVotes()
        {
            if (totalVotes <= 0) return true; // always correct?

            return DBPoll.ClearVotes(pollGuid);
        }

        public bool Delete()
        {
            if (pollGuid == Guid.Empty) return false;

            return DBPoll.Delete(pollGuid);
        }

        public bool Activate()
        {
            if (pollGuid == Guid.Empty) return false;
            if (DateTime.UtcNow < activeFrom) return false;
            if (DateTime.UtcNow > activeTo) return false;
            activated = true;
            return true;
        }

        public bool Deactivate()
        {
            if (pollGuid == Guid.Empty) return false;
            activated = false;
            return true;
        }

        public bool UserHasVoted(SiteUser user)
        {
            if (user == null) return false;
            //if (anonymousVoting) return false;

            return DBPoll.UserHasVoted(pollGuid, user.UserGuid);
        }

        public bool AddToModule(int moduleID)
        {
            return DBPoll.AddToModule(pollGuid, moduleID);
        }

        public bool CopyToNewPoll(out Poll newPoll)
        {
            newPoll = new Poll();
            newPoll.AllowViewingResultsBeforeVoting = allowViewingResultsBeforeVoting;
            newPoll.AnonymousVoting = anonymousVoting;
            newPoll.Question = question;
            newPoll.ShowOrderNumbers = showOrderNumbers;
            newPoll.ShowResultsWhenDeactivated = showResultsWhenDeactivated;
            newPoll.SiteGuid = siteGuid;

            bool result = newPoll.Save();

            if (result)
            {
                PollOption option;
                List<PollOption> pollOptions = PollOption.GetOptionsByPollGuid(pollGuid);
                foreach (PollOption optionToCopy in pollOptions)
                {
                    option = new PollOption();
                    option.Answer = optionToCopy.Answer;
                    option.Order = optionToCopy.Order;
                    option.PollGuid = newPoll.PollGuid;
                    option.Save();
                }
            }

            return result;
        }

        #endregion

        #region Static methods

        public static IDataReader GetPolls(Guid siteGuid)
        {
            return DBPoll.GetPolls(siteGuid);
        }

        public static IDataReader GetActivePolls(Guid siteGuid)
        {
            return DBPoll.GetActivePolls(siteGuid);
        }

        public static IDataReader GetPollsByUser(Guid userGuid)
        {
            if (userGuid == Guid.Empty) return null;
            return DBPoll.GetPollsByUserGuid(userGuid);
        }

        public static bool RemoveFromModule(int moduleID)
        {
            return DBPoll.RemoveFromModule(moduleID);
        }

        

        public static bool DeleteBySite(int siteId)
        {
            return DBPoll.DeleteBySite(siteId);
        }

        #endregion
    }
}
