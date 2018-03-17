using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Names.asset", menuName = "Game/Names", order = 69)]
public class Names : ScriptableObject {

    private static Names Instance { get { return Mods.GetObject<Names>("Names"); } }
    
    public static List<string> MaleFirstNames { get { return Instance.containers[0].Collection; } }
    public static List<string> FemaleFirstNames { get { return Instance.containers[1].Collection; } }
    public static List<string> SurNames { get { return Instance.containers[0].Collection; } }
    
    public List<NameContainer> containers;

    public void CreateContainer()
    {
        containers = new List<NameContainer>()
        {   
            new NameContainer("Male First Names"),
            new NameContainer("Female First Names"),

            new NameContainer("Surnames"),
        };
    }
    
    public static string GetFirstName()
    {
        return GetFirstName(PsychologyManager.GetRandomGender());
    }
    public static string GetFirstName(Genders gender)
    {
        switch (gender)
        {
            case Genders.Male:
                return MaleFirstNames.Random();
            case Genders.Female:
                return FemaleFirstNames.Random();
            default:
                throw new System.NotImplementedException();
        }
    }
    public static string GetMaleFirstName()
    {
        return GetFirstName(Genders.Male);
    }
    public static string GetFemaleFirstName()
    {
        return GetFirstName(Genders.Female);
    }
    public static string GetLastName()
    {
        return SurNames.Random();
    }

    [System.Serializable]
    public class NameContainer
    {
        private NameContainer() { }
        public NameContainer(string containerName)
        {
            _nameType = containerName;
            _collection = new List<string>();
        }

        public string NameType
        {
            get
            {
                return _nameType;
            }
        }
        public void Apply(string[] names)
        {
            for (int i = 0; i < names.Length; i++)
            {
                if (!Collection.Contains(names[i]))
                {
                    Collection.Add(names[i]);
                }
            }
        }

        public List<string> Collection { get { return _collection; } set { _collection = value; } }

        [SerializeField]
        private string _nameType;
        [SerializeField]
        private List<string> _collection;
    }
}
