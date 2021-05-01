using System;
using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Business.Notifications;
using FluentValidation;
using FluentValidation.Results;

namespace Core.Business.Services
{
    public abstract class BaseService
    { 
        private readonly INotifier _notifier;

        protected BaseService(INotifier notifier)
        {
            _notifier = notifier;
        }

        protected void Notification(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notification(error.ErrorMessage);
            }
        }

        protected void Notification(string message)
        {
            _notifier.Handle(new Notification(message));
        }

        protected bool ExecuteValidation<TV, TE>(TV validation, TE entity) where TV : AbstractValidator<TE> where TE : Entity
        {
            var validator = validation.Validate(entity);

            if(validator.IsValid) return true;

            Notification(validator);

            return false;
        }

        public TE AuditColumns<TE, TER>(TE entity, string typeAction, Guid userId, TER entityRelation = null) where TE : Entity where TER : Entity
        {
            if (typeAction == "Create")
            {
                entity.CreatedBy = userId.ToString();
                entity.CreatedDate = DateTime.Now;

                if (entityRelation != null)
                {
                    entityRelation.CreatedBy = userId.ToString();
                    entityRelation.CreatedDate = DateTime.Now;
                }
            }
            else
            {
                entity.UpdatedBy = userId.ToString();
                entity.UpdatedDate = DateTime.Now;

                if (entityRelation != null)
                {
                    entityRelation.UpdatedBy = userId.ToString();
                    entityRelation.UpdatedDate = DateTime.Now;
                }
            }

            return entity;
        }

        public TE AuditColumns<TE>(TE entity, string typeAction, Guid userId) where TE : Entity
        {
            if (typeAction == "Create")
            {
                entity.CreatedBy = userId.ToString();
            }
            else
            {
                entity.UpdatedBy = userId.ToString();
                entity.UpdatedDate = DateTime.Now;
            }

            return entity;
        }
    }
}