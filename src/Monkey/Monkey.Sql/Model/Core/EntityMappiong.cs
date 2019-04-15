using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Monkey.Sql.Model.Core
{
    public interface IEntityMapping
    {
        void Configure(ModelBuilder mb);
    }
    public abstract class EntityMappiong<TEntity> : IEntityMapping
        where TEntity : class
    {
        private EntityTypeBuilder<TEntity> _builder;

        protected EntityTypeBuilder<TEntity> Builder => _builder;
        protected KeyBuilder HasKey(params string[] propertyNames)
        {
            return _builder.HasKey(propertyNames);
        }

        protected KeyBuilder HasAlternateKey(params string[] propertyNames)
        {
            return _builder.HasAlternateKey(propertyNames);
        }

        protected PropertyBuilder Property(string propertyName)
        {
            return _builder.Property(propertyName);
        }

        protected PropertyBuilder<TProperty> Property<TProperty>(string propertyName)
        {
            return _builder.Property<TProperty>(propertyName);
        }

        protected PropertyBuilder Property(Type propertyType, string propertyName)
        {
            return _builder.Property(propertyType, propertyName);
        }

        protected EntityTypeBuilder HasQueryFilter(LambdaExpression filter)
        {
            return _builder.HasQueryFilter(filter);
        }

        protected IndexBuilder HasIndex(params string[] propertyNames)
        {
            return _builder.HasIndex(propertyNames);
        }

        protected ReferenceOwnershipBuilder OwnsOne(string ownedTypeName, string navigationName)
        {
            return _builder.OwnsOne(ownedTypeName, navigationName);
        }

        protected ReferenceOwnershipBuilder OwnsOne(Type ownedType, string navigationName)
        {
            return _builder.OwnsOne(ownedType, navigationName);
        }

        protected EntityTypeBuilder OwnsOne(string ownedTypeName, string navigationName, Action<ReferenceOwnershipBuilder> buildAction)
        {
            return _builder.OwnsOne(ownedTypeName, navigationName, buildAction);
        }

        protected EntityTypeBuilder OwnsOne(Type ownedType, string navigationName, Action<ReferenceOwnershipBuilder> buildAction)
        {
            return _builder.OwnsOne(ownedType, navigationName, buildAction);
        }

        protected CollectionOwnershipBuilder OwnsMany(string ownedTypeName, string navigationName)
        {
            return _builder.OwnsMany(ownedTypeName, navigationName);
        }

        protected CollectionOwnershipBuilder OwnsMany(Type ownedType, string navigationName)
        {
            return _builder.OwnsMany(ownedType, navigationName);
        }

        protected EntityTypeBuilder OwnsMany(string ownedTypeName, string navigationName, Action<CollectionOwnershipBuilder> buildAction)
        {
            return _builder.OwnsMany(ownedTypeName, navigationName, buildAction);
        }

        protected EntityTypeBuilder OwnsMany(Type ownedType, string navigationName, Action<CollectionOwnershipBuilder> buildAction)
        {
            return _builder.OwnsMany(ownedType, navigationName, buildAction);
        }

        protected ReferenceNavigationBuilder HasOne(string relatedTypeName, string navigationName = null)
        {
            return _builder.HasOne(relatedTypeName, navigationName);
        }

        protected ReferenceNavigationBuilder HasOne(Type relatedType, string navigationName = null)
        {
            return _builder.HasOne(relatedType, navigationName);
        }

        protected CollectionNavigationBuilder HasMany(string relatedTypeName, string navigationName = null)
        {
            return _builder.HasMany(relatedTypeName, navigationName);
        }

        protected CollectionNavigationBuilder HasMany(Type relatedType, string navigationName = null)
        {
            return _builder.HasMany(relatedType, navigationName);
        }

        protected IMutableEntityType Metadata => _builder.Metadata;

        protected EntityTypeBuilder<TEntity> HasAnnotation(string annotation, object value)
        {
            return _builder.HasAnnotation(annotation, value);
        }

        protected EntityTypeBuilder<TEntity> HasBaseType(string name)
        {
            return _builder.HasBaseType(name);
        }

        protected EntityTypeBuilder<TEntity> HasBaseType(Type entityType)
        {
            return _builder.HasBaseType(entityType);
        }

        protected EntityTypeBuilder<TEntity> HasBaseType<TBaseType>()
        {
            return _builder.HasBaseType<TBaseType>();
        }

        protected KeyBuilder HasKey(Expression<Func<TEntity, object>> keyExpression)
        {
            return _builder.HasKey(keyExpression);
        }

        protected KeyBuilder HasAlternateKey(Expression<Func<TEntity, object>> keyExpression)
        {
            return _builder.HasAlternateKey(keyExpression);
        }

        protected PropertyBuilder<TProperty> Property<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression)
        {
            return _builder.Property(propertyExpression);
        }

        protected EntityTypeBuilder<TEntity> Ignore(Expression<Func<TEntity, object>> propertyExpression)
        {
            return _builder.Ignore(propertyExpression);
        }

        protected EntityTypeBuilder<TEntity> Ignore(string propertyName)
        {
            return _builder.Ignore(propertyName);
        }

        protected EntityTypeBuilder<TEntity> HasQueryFilter(Expression<Func<TEntity, bool>> filter)
        {
            return _builder.HasQueryFilter(filter);
        }

        protected IndexBuilder HasIndex(Expression<Func<TEntity, object>> indexExpression)
        {
            return _builder.HasIndex(indexExpression);
        }

        protected ReferenceOwnershipBuilder<TEntity, TRelatedEntity> OwnsOne<TRelatedEntity>(string navigationName) where TRelatedEntity : class
        {
            return _builder.OwnsOne<TRelatedEntity>(navigationName);
        }

        protected ReferenceOwnershipBuilder<TEntity, TRelatedEntity> OwnsOne<TRelatedEntity>(Expression<Func<TEntity, TRelatedEntity>> navigationExpression) where TRelatedEntity : class
        {
            return _builder.OwnsOne(navigationExpression);
        }

        protected EntityTypeBuilder<TEntity> OwnsOne<TRelatedEntity>(string navigationName, Action<ReferenceOwnershipBuilder<TEntity, TRelatedEntity>> buildAction) where TRelatedEntity : class
        {
            return _builder.OwnsOne(navigationName, buildAction);
        }

        protected EntityTypeBuilder<TEntity> OwnsOne<TRelatedEntity>(Expression<Func<TEntity, TRelatedEntity>> navigationExpression, Action<ReferenceOwnershipBuilder<TEntity, TRelatedEntity>> buildAction) where TRelatedEntity : class
        {
            return _builder.OwnsOne(navigationExpression, buildAction);
        }

        protected CollectionOwnershipBuilder<TEntity, TRelatedEntity> OwnsMany<TRelatedEntity>(string navigationName) where TRelatedEntity : class
        {
            return _builder.OwnsMany<TRelatedEntity>(navigationName);
        }

        protected CollectionOwnershipBuilder<TEntity, TRelatedEntity> OwnsMany<TRelatedEntity>(Expression<Func<TEntity, IEnumerable<TRelatedEntity>>> navigationExpression) where TRelatedEntity : class
        {
            return _builder.OwnsMany(navigationExpression);
        }

        protected EntityTypeBuilder<TEntity> OwnsMany<TRelatedEntity>(string navigationName, Action<CollectionOwnershipBuilder<TEntity, TRelatedEntity>> buildAction) where TRelatedEntity : class
        {
            return _builder.OwnsMany(navigationName, buildAction);
        }

        protected EntityTypeBuilder<TEntity> OwnsMany<TRelatedEntity>(Expression<Func<TEntity, IEnumerable<TRelatedEntity>>> navigationExpression, Action<CollectionOwnershipBuilder<TEntity, TRelatedEntity>> buildAction) where TRelatedEntity : class
        {
            return _builder.OwnsMany(navigationExpression, buildAction);
        }

        protected ReferenceNavigationBuilder<TEntity, TRelatedEntity> HasOne<TRelatedEntity>(string navigationName) where TRelatedEntity : class
        {
            return _builder.HasOne<TRelatedEntity>(navigationName);
        }

        protected ReferenceNavigationBuilder<TEntity, TRelatedEntity> HasOne<TRelatedEntity>(Expression<Func<TEntity, TRelatedEntity>> navigationExpression = null) where TRelatedEntity : class
        {
            return _builder.HasOne(navigationExpression);
        }

        protected CollectionNavigationBuilder<TEntity, TRelatedEntity> HasMany<TRelatedEntity>(string navigationName) where TRelatedEntity : class
        {
            return _builder.HasMany<TRelatedEntity>(navigationName);
        }

        protected CollectionNavigationBuilder<TEntity, TRelatedEntity> HasMany<TRelatedEntity>(Expression<Func<TEntity, IEnumerable<TRelatedEntity>>> navigationExpression = null) where TRelatedEntity : class
        {
            return _builder.HasMany(navigationExpression);
        }

        protected EntityTypeBuilder<TEntity> HasChangeTrackingStrategy(ChangeTrackingStrategy changeTrackingStrategy)
        {
            return _builder.HasChangeTrackingStrategy(changeTrackingStrategy);
        }

        protected EntityTypeBuilder<TEntity> UsePropertyAccessMode(PropertyAccessMode propertyAccessMode)
        {
            return _builder.UsePropertyAccessMode(propertyAccessMode);
        }

        protected DataBuilder<TEntity> HasData(params TEntity[] data)
        {
            return _builder.HasData(data);
        }

        protected DataBuilder<TEntity> HasData(IEnumerable<TEntity> data)
        {
            return _builder.HasData(data);
        }

        protected DataBuilder<TEntity> HasData(params object[] data)
        {
            return _builder.HasData(data);
        }

        protected DataBuilder<TEntity> HasData(IEnumerable<object> data)
        {
            return _builder.HasData(data);
        }

        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            _builder = builder;
            Configure();
        }
        protected abstract void Configure();

        public void Configure(ModelBuilder mb)
        {
            Configure(mb.Entity<TEntity>());
        }
    }
}