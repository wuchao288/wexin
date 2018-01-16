using Hidistro.Entities.Config;
using Hidistro.SqlDal.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.ControlPanel.Store
{
    public class AppConfigHelper
    {
        public static AppConfigModel GetAppConfig() {
            AppConfigDao configDao = new AppConfigDao();
            return configDao.GetConfig();
        }

        public static void SaveConfigModel(AppConfigModel model)
        {
            AppConfigDao configDao = new AppConfigDao();
            configDao.SaveConfigModel(model);
        }
    }
}
