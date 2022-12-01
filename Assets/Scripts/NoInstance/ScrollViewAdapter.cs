using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewAdapter : MonoBehaviour
{/*
    [SerializeField] private RectTransform prefab; //item
    [SerializeField] private RectTransform content;
    [SerializeField] private TMP_InputField countText_input;
    
    public void UpdateItems()
    {
        int modelsCount = 0;
        int.TryParse(this.countText_input.text.Trim(), out modelsCount);
        StartCoroutine(GetItems(modelsCount, results => OnReceivedModels(results)));
    }
    void OnReceivedModels(TestItemModel[] models)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        foreach (var model in models)
        {
            var instance = GameObject.Instantiate(this.prefab.gameObject) as GameObject;
            instance.transform.SetParent(content, false);
            InitializeItemView(instance, model);
        }
    }
    void InitializeItemView(GameObject viewGameObject, TestItemModel model)
    {
        TestItemView view = new TestItemView(viewGameObject.transform);
        view.titleText.text = model.title;
        view.clickButton.GetComponentInChildren<TextMeshProUGUI>().text = model.buttomText;

        view.clickButton.onClick.AddListener(
            () =>//ловим по событию
            {
                Debug.Log(view.titleText.text + "is clicked!");
            }
        );
    }
    IEnumerator GetItems (int count, System.Action<TestItemModel[]> callback)
    {
        yield return new WaitForSeconds(1f);
        var results = new TestItemModel[count];
        for (int i = 0; i < count; i++)
        {
            results[i] = new TestItemModel();
            results[i].title = "Item" + i;
            results[i].buttomText = "Button" + i;
        }
        callback(results);
    }
    public class TestItemView
    {
        /*public TextMeshProUGUI titleText;
        public Button clickButton;
        public TestItemView(Transform rootView)
        {
            titleText = rootView.Find("TitleText").GetComponent<TextMeshProUGUI>();
            clickButton = rootView.Find("ClickButton").GetComponent<Button>();
        }*/
    }
    public class TestItemModel
    {
        public string title;
        public string buttomText;
    }

