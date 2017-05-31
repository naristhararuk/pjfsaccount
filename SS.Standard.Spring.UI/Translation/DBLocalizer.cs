using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading;

using Spring.Globalization;
using Spring.Context;
using Spring.Util;
using Spring.Expressions;
using Spring.Web.UI.Controls;

using SS.SU.Query;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.BLL;

namespace SS.Standard.UI.Spring.Translation
{
    public class DBLocalizer : AbstractLocalizer
    {
        public IResourceCache resourceCache = new NullResourceCache();

        /// <summary>
        /// Gets or sets the resource cache instance.
        /// </summary>
        /// <value>The resource cache instance.</value>
        public new IResourceCache ResourceCache
        {
            get { return resourceCache; }
            set { resourceCache = value; }
        }

		public ISuGlobalTranslateService SuGlobalTranslateService { get; set; }

        /// <summary>
        /// Applies resources of the specified culture to the specified target object.
        /// </summary>
        /// <param name="target">Target object to apply resources to.</param>
        /// <param name="messageSource"><see cref="IMessageSource"/> instance to retrieve resources from.</param>
        /// <param name="culture">Resource culture to use for resource lookup.</param>
        public new void ApplyResources(object target, IMessageSource messageSource, CultureInfo culture)
        {
            AssertUtils.ArgumentNotNull(target, "target");
            AssertUtils.ArgumentNotNull(culture, "culture");

			string programCode = string.Empty; 
			
			if (target is SS.Standard.UI.BasePage) 
				programCode = ((SS.Standard.UI.BasePage)target).ProgramCode;
			if (target is SS.Standard.UI.BaseUserControl) 
				programCode = ((SS.Standard.UI.BaseUserControl)target).ProgramCode;
			if (target is SS.Standard.UI.BaseMaster) 
				programCode = ((SS.Standard.UI.BaseMaster)target).ProgramCode;
				
            IList resources = GetResources(target, messageSource, culture);
            foreach (Resource resource in resources)
            {
				try
				{
					resource.Target.SetValue(target, null, resource.Value);
				}
				catch (Exception) 
				{
					GlobalTranslate translate = resource.Value as GlobalTranslate;
					if (!(string.IsNullOrEmpty(programCode)) && !(string.IsNullOrEmpty(translate.TranslateControl)))
					{
						SuGlobalTranslateService.DeleteByProgramCodeAndControl(programCode, translate.TranslateControl);
					}
					resources.Remove(resource);
				}
            }
        }

        /// <summary>
        /// Applies resources to the specified target object, using current thread's uiCulture to resolve resources.
        /// </summary>
        /// <param name="target">Target object to apply resources to.</param>
        /// <param name="messageSource"><see cref="IMessageSource"/> instance to retrieve resources from.</param>
        public new void ApplyResources(object target, IMessageSource messageSource)
        {
            AssertUtils.ArgumentNotNull(target, "target");
            ApplyResources(target, messageSource, Thread.CurrentThread.CurrentUICulture);
        }

        /// <summary>
        /// Returns a list of <see cref="Resource"/> instances that should be applied to the target.
        /// </summary>
        /// <param name="target">Target to get a list of resources for.</param>
        /// <param name="messageSource"><see cref="IMessageSource"/> instance to retrieve resources from.</param>
        /// <param name="culture">Resource locale.</param>
        /// <returns>A list of resources to apply.</returns>
        private IList GetResources(object target, IMessageSource messageSource, CultureInfo culture)
        {
            IList resources = resourceCache.GetResources(target, culture);

            if (resources == null)
            {
                resources = LoadResources(target, messageSource, culture);
                resourceCache.PutResources(target, culture, resources);
            }

            return resources;
        }

        /// <summary>
        /// Loads resources from the storage and creates a list of <see cref="Resource"/> instances that should be applied to the target.
        /// </summary>
        /// <param name="target">Target to get a list of resources for.</param>
        /// <param name="messageSource"><see cref="IMessageSource"/> instance to retrieve resources from.</param>
        /// <param name="culture">Resource locale.</param>
        /// <returns>A list of resources to apply.</returns>
        protected override IList LoadResources(object target, IMessageSource messageSource, CultureInfo culture)
        {
            IList resources;
            resources = new ArrayList();
            /*            if (resourceValue is String && ((String)resourceValue).StartsWith("$messageSource"))
                        {
                            resourceValue = messageSource.GetResourceObject(((String)resourceValue).Substring(15), culture);
                        }*/
            if (target is SS.Standard.UI.BasePage)
            {
                string programCode = ((SS.Standard.UI.BasePage)target).ProgramCode;


                IList<GlobalTranslate> globalTranslates = QueryProvider.SuGlobalTranslateQuery.LoadProgramResources(programCode, culture.Name);

                foreach (GlobalTranslate globalTranslate in globalTranslates)
                {
                    resources.Add(new Resource(Expression.Parse(globalTranslate.TranslateControl), globalTranslate.TranslateWord));
                }
            }
            else if (target is SS.Standard.UI.BaseUserControl)
            {
                string programCode = ((SS.Standard.UI.BaseUserControl)target).ProgramCode;


                IList<GlobalTranslate> globalTranslates = QueryProvider.SuGlobalTranslateQuery.LoadProgramResources(programCode, culture.Name);

                foreach (GlobalTranslate globalTranslate in globalTranslates)
                {
                    resources.Add(new Resource(Expression.Parse(globalTranslate.TranslateControl), globalTranslate.TranslateWord));
                }
            }
            else if (target is SS.Standard.UI.BaseMaster)
            {
                string programCode = ((SS.Standard.UI.BaseMaster)target).ProgramCode;


                IList<GlobalTranslate> globalTranslates = QueryProvider.SuGlobalTranslateQuery.LoadProgramResources(programCode, culture.Name);

                foreach (GlobalTranslate globalTranslate in globalTranslates)
                {
                    resources.Add(new Resource(Expression.Parse(globalTranslate.TranslateControl), globalTranslate.TranslateWord));
                }
            }

            return resources;
        }
    }
}
