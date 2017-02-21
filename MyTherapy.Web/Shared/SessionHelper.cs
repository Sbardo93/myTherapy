using System;

namespace MyTherapy.Web
{
    /// <summary>
    /// Classe gestore degli oggetti in Sessione e Cache
    /// 
    /// Per censire un nuovo oggetto in Sessione:
    ///     1. creare la SessionKey associata con il Primo Costruttore
    ///         ESEMPIO: public static readonly SessionKeys Intervento = new SessionKeys("Intervento");
    ///     2. accedere all'oggetto con i metodi Get, GetValue, Set, SetValue della Classe SessionHelper
    ///         ESEMPIO: SessionHelper.Get(SessionKeys.Intervento);
    ///                  SessionHelper.Set(SessionKeys.Intervento, value);
    /// 
    /// Per censire un nuovo oggetto in Cache:
    ///     1. creare la SessionKey associata con il Secondo o Terzo Costruttore
    ///         ESEMPIO: public static readonly SessionKeys Intervento = new SessionKeys("ParametriTariffe", true);
    ///                  ..Associa alla chiave un metodo per il recupero dell'oggetto   
    ///                  public static readonly SessionKeys ParametriTariffe = new SessionKeys("ParametriTariffe", true,
    ///                     delegate()
    ///                     {
    ///                         SessionHelper.Set(SessionKeys.ParametriTariffe,
    ///                             BusinessLogic.GestioneLineeServizio.GetParametriTariffe());  !!! IMPORTANTE: Ricordare di effettuare la Set dell'oggetto all'interno del Delegato
    ///                     }
    ///                  );
    ///     2. accedere all'oggetto con gli stessi metodi utilizzabili per gli oggetti in sessione (Get, GetValue, Set, SetValue)
    ///        ..se la chiave a cui si accede ha un metodo Delegato è possibile non effettuare la Set dell'oggetto in quanto il Delegato viene chiamato accedendo all'oggetto NULL
    /// 
    /// </summary>
    public class SessionHelper
    {
        private static System.Web.Caching.Cache CACHE { get { return System.Web.HttpContext.Current.Cache; } }
        private static System.Web.SessionState.HttpSessionState SESSION { get { return System.Web.HttpContext.Current.Session; } }
        
        private static object CacheGet(SessionKeys key)
        {
            return CACHE.Get(key.Value);
        }
        public static object CacheSet(SessionKeys key, object obj)
        {
            int defaultTime = 15;

            return CACHE.Add(
                key.Value, obj,
                null, DateTime.Now.AddMinutes(defaultTime), TimeSpan.Zero,
                System.Web.Caching.CacheItemPriority.Normal,
                null
                );
        }
        public static object Get(SessionKeys key)
        {
            if (SESSION == null)
                return null;

            if (key.IsInCache)
            {
                object obj = CacheGet(key);

                if (obj == null && key.LoadFromDBhandler != null)
                {
                    key.LoadFromDBhandler();
                    obj = CacheGet(key);
                }
                return obj;
            }
            return SESSION[key.Value];
        }
        public static string GetValue(SessionKeys key)
        {
            object obj = Get(key);

            return (obj + string.Empty).ToString();
        }
        public static void Set(SessionKeys key, object obj)
        {
            if (key.IsInCache)
            {
                CacheSet(key, obj);
            }
            else
            {
                SESSION.Add(key.Value, obj);
            }
        }
        public static void SetValue(SessionKeys key, object obj)
        {
            if (key.IsInCache)
            {
                CacheSet(key, obj.ToString());
            }
            else
            {
                SESSION.Add(key.Value, obj.ToString());
            }
        }
        public static void Delete(SessionKeys key)
        {
            if (key.IsInCache)
            {
                CACHE.Remove(key.Value);
            }
            else
            {
                SESSION.Remove(key.Value);
            }
        }
        public static bool Contains(SessionKeys key)
        {
            return Get(key) != null;
        }

        #region Metodi SessionKeys

        /// SESSIONE
        //public static object GET_CHIAVE_SESSIONE()
        //{
        //    if (Get(SessionKeys.CHIAVE) != null)
        //    {
        //        return Get(SessionKeys.CHIAVE);
        //    }
        //    else
        //    {
        //        object obj = BusinessLogic.GestioneUtility...
        //        if (obj != null)
        //        {
        //            SessionHelper.Set(SessionKeys.CHIAVE, obj);
        //        }
        //        return obj;
        //    }
        //}

        #endregion
    }


    /// <summary>
    /// Classe contenente le chiavi degli oggetti in Sessione o in Cache
    /// </summary>
    public class SessionKeys
    {
        #region Chiavi in Sessione

        #endregion

        #region Chiavi in Cache

        #endregion

        #region Costruttori SessionKey
        /// <summary>
        /// Crea una chiave per un oggetto in Sessione
        /// </summary>
        /// <param name="value">Valore chiave</param>
        private SessionKeys(string value)
        {
            Value = value;
            IsInCache = false;
        }
        /// <summary>
        /// Crea una chiave per un oggetto in Cache
        /// </summary>
        /// <param name="value">Valore chiave</param>
        /// <param name="isInCache">Indica se l'oggetto sarà salvato in Cache</param>
        private SessionKeys(string value, bool isInCache)
        {
            Value = value;
            IsInCache = isInCache;
            LoadFromDBhandler = null;
        }
        /// <summary>
        /// Crea una chiave per un oggetto in Cache con un metodo di caricamento
        /// </summary>
        /// <param name="value">Valore chiave</param>
        /// <param name="isInCache">Indica se l'oggetto sarà salvato in Cache</param>
        /// <param name="lfdbh">Metodo delegato per il caricamento dell'oggetto</param>
        private SessionKeys(string value, bool isInCache, LoadFromDB lfdbh)
        {
            Value = value;
            IsInCache = isInCache;
            LoadFromDBhandler = lfdbh;
        }
        #endregion

        public string Value { get; private set; }
        public bool IsInCache { get; private set; }
        public delegate void LoadFromDB();
        public LoadFromDB LoadFromDBhandler;
    }
}