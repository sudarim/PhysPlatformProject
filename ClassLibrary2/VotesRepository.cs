using System;
using System.Collections.Generic;
using System.Linq;
using PhysPlatformProject.DomainModels;

namespace PhysPlatformProject.Repositories
{
    public interface IVotesRepository
    {
        void UpdateVote(int aid,int uid, int value);


    }
    class VotesRepository:IVotesRepository
    {
        PhysPlatformDatabaseDbContext db;
        public VotesRepository()
        {
            db = new PhysPlatformDatabaseDbContext();
        }
        public void UpdateVote(int aid, int uid, int value)
        {
            int updateValue;
            if (value > 0) updateValue = 1;
            else if (value < 0) updateValue = -1;
            else updateValue = 0;

            Vote vote = db.Votes.Where(temp => temp.AnswerID == aid && temp.UserID == uid).FirstOrDefault();
            if (vote != null)
            {
                vote.VoteValue = updateValue;

            }
            else
            {
                Vote newVote = new Vote()
                {
                    AnswerID = aid,
                    UserID = uid,
                    VoteValue = updateValue
                };
                db.Votes.Add(newVote);
            }
            db.SaveChanges();
        }

    }
}
