/// Author:				Christian Fredh
/// Created:			2007-04-25
/// Last Modified:		2009-02-01
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software. 

using System;
using System.Collections.Generic;
using System.Data;
using PollFeature.Data;

namespace PollFeature.Business
{
    /// <summary>
    /// Represents an instance of a polloption
    /// </summary>
    public class PollOption
    {
        #region Constructors


        public PollOption()
        { }

        public PollOption(Guid optionGuid)
        {
            if (optionGuid != Guid.Empty)
            {
                GetPollOption(optionGuid);
            }
        }


        #endregion

        #region Private Properties

        private Guid pollGuid = Guid.Empty;
        private Poll poll = null;
        private Guid optionGuid = Guid.Empty;
        private String answer = String.Empty;
        private int votes = 0;
        private int order = 1;
        private Guid newOptionGuid = Guid.NewGuid();


        #endregion

        #region Public Properties

        public Guid PollGuid
        {
            get { return pollGuid; }
            set { pollGuid = value; }
        }

        public Guid OptionGuid
        {
            get { return optionGuid; }
        }

        public String Answer
        {
            get { return answer; }
            set { answer = value; }
        }

        public int Votes
        {
            get { return votes; }
        }

        public double? VotePercentage
        {
            get
            {
                if (Poll.TotalVotes == 0) return null;

                return (double)votes / (double)Poll.TotalVotes * 100;
            }
                
        }

        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        public Poll Poll
        {
            get
            {
                if (poll == null) poll = new Poll(pollGuid);
                return poll;
            }
        }

        #endregion


        #region Private Methods

        private void GetPollOption(Guid optionGuid)
        {
            using (IDataReader reader = DBPollOption.GetPollOption(optionGuid))
            {
                if (reader.Read())
                {
                    this.pollGuid = new Guid(reader["PollGuid"].ToString());
                    this.answer = reader["Answer"].ToString();
                    this.order = int.Parse(reader["Order"].ToString());
                    this.votes = int.Parse(reader["Votes"].ToString());

                    this.optionGuid = optionGuid;
                }
            }
        }

        private bool Create()
        {
            optionGuid = newOptionGuid;

            int rowsAffected = DBPollOption.Add(
                optionGuid,
                pollGuid, 
                answer,
                order);

            return (rowsAffected > 0);
        }

        private bool Update()
        {
            return DBPollOption.Update(
                optionGuid, 
                answer,
                order);
        }

        #endregion


        #region Public Methods

        public bool Save()
        {
            if (optionGuid == Guid.Empty) return Create();
            return Update();
        }

        public bool IncrementVotes(Guid userGuid)
        {
            if (optionGuid == Guid.Empty) return false;

            return DBPollOption.IncrementVotes(pollGuid, optionGuid, userGuid);
        }

        public bool Delete()
        {
            if (optionGuid == Guid.Empty) return false;

            return DBPollOption.Delete(optionGuid);
        }

        #endregion

        #region Static Methods

        private static List<PollOption> LoadListFromReader(IDataReader reader)
        {
            List<PollOption> pollOptionList = new List<PollOption>();
            try
            {
                while (reader.Read())
                {
                    PollOption pollOption = new PollOption();
                    pollOption.optionGuid = new Guid(reader["OptionGuid"].ToString());
                    pollOption.pollGuid = new Guid(reader["PollGuid"].ToString());
                    pollOption.answer = reader["Answer"].ToString();
                    pollOption.votes = Convert.ToInt32(reader["Votes"]);
                    pollOption.order = Convert.ToInt32(reader["Order"]);
                    pollOptionList.Add(pollOption);

                }
            }
            finally
            {
                reader.Close();
            }

            return pollOptionList;

        }

        public static List<PollOption> GetOptionsByPollGuid(Guid pollGuid)
        {
            IDataReader reader = DBPollOption.GetPollOptions(pollGuid);
            return LoadListFromReader(reader);
        }

        #endregion

    }
}
