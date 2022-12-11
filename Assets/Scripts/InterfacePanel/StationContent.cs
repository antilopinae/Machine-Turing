using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StationContent : MonoBehaviour
{
    [SerializeField] private RectTransform thisContent;
    public static RectTransform content;
    [SerializeField] private GameObject el_station;
    [SerializeField] private GameObject el_symbol;
    [SerializeField] private GameObject el_add_station;
    [SerializeField] private GameObject el_add_symbol;
    [SerializeField] private Button buttonPlayGame;
    private static List<ElementStation> elementStations = new List<ElementStation>();
    private static List<string[][]> Symbols=new List<string[][]>();
    private static List<string> Stations = new List<string>();
    private static bool start = true;

    public void Start()
    {
        //Base add SQLite
        content = thisContent;
        buttonPlayGame.onClick.AddListener(()=> { OnReceivedStations(); ActGame(); });
        //elementStations.Add(new ElementStation(0));
        OnReceivedStations();
    }
    public void OnReceivedStations()
    {
        GameObject Instans(GameObject prefab)
        {
            var instance = Instantiate(prefab) as GameObject;
            instance.transform.SetParent(content, false);
            return instance;
        }
        if (start)
        {
            foreach (string name_state in Stations)
            {
                elementStations.Add(new ElementStation(elementStations.Count()));

                foreach (string[] name_symbol in Symbols[elementStations.Count()-1])
                {
                    elementStations[elementStations.Count() - 1].AddSymbol(name_symbol);
                }
            }
            start = false;
        }
        else { Symbols.Clear(); Stations.Clear();}
        bool isFirst = true;
        /*for (int i=0; i<elementStations.Count(); i++)
        {
            bool exepshin = false;
            Symbols.Add(elementStations[i].ReturnSymbs());
            Stations.Add(elementStations[i].GetName());
            elementStations[i].InitializeExepshin(false);

            if (Exepshin.error_state.Count() != 0 && Exepshin.error_state.Contains(i))
            {
                exepshin = true;
                elementStations[i].InitializeExepshin(exepshin);
            }
            for (int c=0;c< elementStations[i].symbolsElements.Count(); c++)
            {
                elementStations[i].symbolsElements[c].InitializeExepshin(false);
                if (isFirst &&exepshin && Exepshin.error_symbol.Count()!=0 && Exepshin.error_symbol.Contains(c))
                {
                    elementStations[i].symbolsElements[c].InitializeExepshin(exepshin);
                    isFirst = false;
                }
                elementStations[i].symbolsElements[c].Delete();
            }
            elementStations[i].Delete();
        }*/

        foreach(Transform child in content)
        {
            Destroy(child.gameObject);
        }
        foreach (ElementStation elementStation in elementStations)
        {
            elementStation.InitializeStation(Instans(el_station));

            foreach (ElementSymbol symbol in elementStation.symbolsElements)
            {
                symbol.InitializeSymbol(Instans(el_symbol));
            }
            elementStation.CollapseSymbols(elementStation.isActive);
            GameObject buttonCollapse = Instans(el_add_symbol);
            buttonCollapse.SetActive(elementStation.isActive);
            buttonCollapse.GetComponent<Button>().onClick.AddListener(() => {elementStation.AddSymbol(); OnReceivedStations(); });
        }
        Instans(el_add_station).GetComponent<Button>().onClick.AddListener(() => { elementStations.Add(new ElementStation(elementStations.Count)); OnReceivedStations(); });
    }

    private class ElementStation
    {
        private int index;
        private string name = "";
        private GameObject station;
        private TMP_InputField station_input;
        private Button collapseButton;
        public List<ElementSymbol> symbolsElements;
        public bool isActive=true;
        private bool initialize = false;
        private bool exepshin = false;

        public void Delete()
        {
            if (initialize)
            {
                GetName();
                Destroy(station);
            }
        }
        public void AddSymbol()
        {
            var symbol = new ElementSymbol(index);
            symbolsElements.Add(symbol);
            InitializeCount();
        }
        public void AddSymbol(string[] name_symbs)
        {
            var symbol = new ElementSymbol(index);
            symbolsElements.Add(symbol);
            symbol.SetText(name_symbs);
            InitializeCount();
        }
        public void InitializeCount()
        {
            foreach (ElementSymbol symbol in symbolsElements)
            {
                symbol.SetId(symbolsElements.IndexOf(symbol));
            }
        }
        public void DeleteOnObject(int id)
        {
            symbolsElements.RemoveAt(id);
            InitializeCount();
        }
        public string[][] ReturnSymbs()
        {
            string[][] symbols;
            symbols = new string[symbolsElements.Count][];
            for (int i=0; i<symbolsElements.Count; ++i)
            {
                symbols[i] = symbolsElements[i].GetText();
            }
            return symbols;
        }
        public void CollapseSymbols(bool bl)
        {
            foreach (ElementSymbol symbol in symbolsElements)
            {
                symbol.SetAct(bl);
            }
            collapseButton.onClick.AddListener(() => { collapseButton.onClick.RemoveAllListeners(); isActive = !bl;InitializeContent(); });
        }
        public ElementStation(int index)
        {
            symbolsElements = new List<ElementSymbol>();
            this.index = index;
        }
        public void SetName(string str="")
        {
            if (initialize)
            {
                station_input.text = str;
            }
        }
        public string GetName()
        {
            name = station_input.text;
            return name;
        }
        public void InitializeExepshin(bool exepshin)
        {
            this.exepshin = exepshin;
        }
        public void InitializeStation(GameObject station)
        {
            this.station = station;
            if (exepshin) station.GetComponent<ExepshinState>().Exepshin();
            this.station_input = station.transform.GetChild(0).GetComponent<TMP_InputField>();
            this.collapseButton = station.transform.GetChild(1).GetComponent<Button>();
            this.collapseButton.gameObject.GetComponent<CollapseButton>().Collapsed(isActive);
            this.station_input.text = name;
            collapseButton.onClick.AddListener(() => { collapseButton.onClick.RemoveAllListeners(); CollapseSymbols(false); isActive = false; InitializeContent(); });
            initialize = true;
    }
        private void InitializeContent()
        {
            StationContent.content.GetComponent<StationContent>().OnReceivedStations();
        }
    }

    private class ElementSymbol
    {
        private int id;
        private int index;
        private GameObject symbol;
        private TMP_InputField name_input;
        private TMP_InputField move_input;
        private TMP_InputField nextStation;
        private Button removeButton;
        private string[] texts = { "A", "R", "Q2" };
        private bool initialize = false;
        private bool exepshin = false;
        public ElementSymbol(int index)
        {
            this.index = index;
        }
        public void InitializeExepshin(bool exepshin)
        {
            this.exepshin = exepshin;
        }
        public void InitializeSymbol(GameObject symbol)
        {
            this.symbol = symbol;
            if (exepshin) symbol.GetComponent<ExepshinSymbol>().Exepshin();
            this.name_input = getComponent(0);
            this.move_input = getComponent(1);
            this.nextStation = getComponent(2);
            this.removeButton = symbol.transform.GetChild(1).GetComponent<Button>();
            removeButton.onClick.AddListener(() => { Debug.Log(getId()); elementStations[this.index].InitializeCount(); elementStations[this.index].DeleteOnObject(getId()); InitializeContent(); });
            SetText(null);
            TMP_InputField getComponent(int ind)
            {
                return symbol.transform.GetChild(0).GetChild(ind).GetComponent<TMP_InputField>();
            }
            initialize = true;
    }
        private void InitializeContent()
        {
            StationContent.content.GetComponent<StationContent>().OnReceivedStations();
        }
        
        public void SetAct(bool bl)
        {
            if (symbol!= null)
            {
                symbol.SetActive(bl);
            }
        }
        public string[] GetText()
        {
            texts[0]=name_input.text;
            texts[1]=move_input.text;
            texts[2]=nextStation.text;
            return texts;
        }
        public void SetText(string[] texts)
        {
            if (texts==null) texts = this.texts;
            name_input.text = texts[0];
            move_input.text = texts[1];
            nextStation.text = texts[2];
        }
        public void Delete()
        {
            if (initialize)
            {
                GetText();
                Destroy(symbol);
            }
        }
        public void SetId(int id)
        {
            this.id = id;
        }
        private int getId()
        {
            return id;
        }
    }
    public class Base
    {
        private static List<string> state_names;
        private static List<string[][]> state_types;
        public static Exepshin exepshin;
        public Base(List<string> state_names, List<string[][]> state_types)
        {
            Base.state_names = state_names;
            Base.state_types = state_types;
        }
        public Table SearchSymbol(string name_state, string name_symbol, int ind_state, int ind_symb)
        {
            List<int> searched_state=new List<int>();
            for (int i = 0; i < state_names.Count; i++)
            {
                if (state_names[i] ==name_state && name_state!="") searched_state.Add(i);
            }
            if (searched_state.Count() == 1)
            {
                var symbols = state_types[searched_state[0]];

                List<int> searched_symb = new List<int>();
                for (int i = 0; i < symbols.Length; i++)
                {
                    if (symbols[i][0] == name_symbol && name_symbol != "") searched_symb.Add(i);
                }
                if (searched_symb.Count() == 1)
                {
                    if (symbols[searched_symb[0]][1]!="" && symbols[searched_symb[0]][2] != "")
                    {
                        string[] command = { "L", "R", "!" };
                        if (command.Contains(symbols[searched_symb[0]][1]))
                        {
                            exepshin = new Exepshin();
                            return new Table(searched_state[0], searched_symb[0]);
                        }
                        exepshin = new Exepshin(searched_state[0], searched_symb);
                    }
                    else 
                    {
                        exepshin = new Exepshin(searched_state[0],searched_symb);
                    }
                }
                else if (searched_symb.Count() == 0)
                {

                    exepshin = new Exepshin(ind_state,searched_state[0], ind_symb);
                }
                else
                {
                    exepshin = new Exepshin(searched_state[0],searched_symb);
                }

            }
            else if (searched_state.Count()==0)
            {
                exepshin = new Exepshin(ind_state, ind_symb);
            }
            else
            {
                exepshin = new Exepshin(searched_state);
            }
            return null;
        }
        public class Table
        {
            public int state_index { get; }
            public int symbol_index { get; }
            public Table(int state_index, int symbol_index)
            {
                this.state_index = state_index;
                this.symbol_index = symbol_index;
            }
            public string ReturnStateByIndex(int index_state)
            {
                return state_names[index_state];
            }
            public string[] ReturnSymbolByIndex(int index_symbol)
            {
                return state_types[state_index][index_symbol];
            }
        }
    }

    public class Exepshin
    {
        public static List<int> error_state = new List<int>(); 
        public static List<int> error_symbol = new List<int>();
        private void Clean()
        {
            Exepshin.error_state.Clear();
            error_symbol.Clear();
        }
        public Exepshin()
        {
            Clean();
        }
        public Exepshin(int error_state, List<int> error_symbol)
        {
            Clean();
            Exepshin.error_state.Add(error_state);
            Exepshin.error_symbol = error_symbol;
        }
        public Exepshin(int error_state_st,int error_state_fin, int error_symbol)
        {
            Clean();
            Exepshin.error_symbol.Add(error_symbol);
            Exepshin.error_state.Add(error_state_st);
            Exepshin.error_state.Add(error_state_fin);
        }
        public Exepshin(List<int> error_state)
        {
            Clean();
            Exepshin.error_state = error_state;
        }
        public Exepshin(int state, int symbol)
        {
            Clean();
            Exepshin.error_state.Add(state);
            Exepshin.error_symbol.Add(symbol);
        }
    }



    public static Action<Base> onBase;
    public static void ActGame()
    {
        var bas = new Base(Stations,Symbols);
        onBase?.Invoke(bas);
    }

}