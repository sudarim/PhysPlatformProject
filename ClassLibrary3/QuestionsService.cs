﻿using System;
using System.Collections.Generic;
using System.Linq;
using PhysPlatformProject.DomainModels;
using PhysPlatformProject.ViewModels;
using PhysPlatformProject.Repositories;
using AutoMapper;
using AutoMapper.Configuration;

namespace PhysPlatformProject.ServiceLayer
{
    public interface IQuestionsService
    {
        void InsertQuestion(NewQuestionViewModel qvm);
        void UpdateQuestionDetails(EditQuestionViewModel qvm);
        void UpdateQuestionVotes(int qid, int value);
        void UpdateQuestionAnswerCount(int qid, int value);
        void UpdateQuestionViewsCount(int qid, int value);
        void DeleteQuestion(int qid);
        List<QuestionViewModel> GetQuestions();
        QuestionViewModel GetQuestionByQuestionID(int QuestionID, int UserID);

    }
    public class QuestionsService : IQuestionsService
    {
        IQuestionRepository qr;
        public QuestionsService()
        {
            qr = new QuestionsRepository();
        }
       public void InsertQuestion(NewQuestionViewModel qvm)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<NewQuestionViewModel, Question>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            Question q = mapper.Map<NewQuestionViewModel, Question>(qvm);
            qr.InsertQuestion(q);
        }
        public void UpdateQuestionDetails(EditQuestionViewModel qvm)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<EditQuestionViewModel, Question>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            Question q = mapper.Map<EditQuestionViewModel, Question>(qvm);
            qr.UpdateQuestionDetails(q);

        }
        public void UpdateQuestionVotes(int qid, int value)
        {
            qr.UpdateQuestionVotesCount(qid, value);
        }

        public void UpdateQuestionAnswerCount(int qid, int value)
        {
            qr.UpdateQuestionAnswersCount(qid, value);
        }
        public void UpdateQuestionViewsCount(int qid, int value)
        {
            qr.UpdateQuestionViewsCount(qid, value);
        }
        public void DeleteQuestion(int qid)
        {
            qr.DeleteQuestion(qid);
        }
       public List<QuestionViewModel> GetQuestions()
        {
            List<Question> q = qr.GetQuestions();
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<Question, QuestionViewModel>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            List<QuestionViewModel> qvm = mapper.Map<List<Question>, List<QuestionViewModel>>(q);
            return qvm;
        }
        public QuestionViewModel GetQuestionByQuestionID(int QuestionID, int UserID)
        {
            QuestionViewModel qvm = null;
            Question q = qr.GetQuestionByQuestionID(QuestionID).FirstOrDefault();
            if (q != null)
            {
                var config = new MapperConfiguration(cfg => { cfg.CreateMap<Question, QuestionViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                qvm = mapper.Map<Question, QuestionViewModel>(q);
                foreach(var item in qvm.Answers){
                    item.CurrentUserVoteType = 0;
                    VoteViewModel vote = item.Votes.Where(temp => temp.UserID == UserID).FirstOrDefault();
                    if (vote != null) {
                        item.CurrentUserVoteType = vote.VoteValue;
                    }
                }
            }
            return qvm;

        }
    }
}
