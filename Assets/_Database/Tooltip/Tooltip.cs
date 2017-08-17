using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RPG.Database
{
    public class Tooltip : MonoBehaviour
    {
        public Item item;

        //GUI
        [SerializeField]
        public Image tooltipBackground;
        [SerializeField]
        public Text tooltipNameText;
        [SerializeField]
        public Text tooltipDescText;
        [SerializeField]
        public Text tooltipAttributeText;

        //Tooltip Settings
        [SerializeField]
        public bool showTooltip;
        [SerializeField]
        private bool tooltipCreated;
        [SerializeField]
        private int tooltipWidth;
        [SerializeField]
        public int tooltipHeight;
        [SerializeField]
        private bool showTooltipIcon;
        [SerializeField]
        private int tooltipIconPosX;
        [SerializeField]
        private int tooltipIconPosY;
        [SerializeField]
        private int tooltipIconSize;
        [SerializeField]
        private bool showTooltipName;
        [SerializeField]
        private int tooltipNamePosX;
        [SerializeField]
        private int tooltipNamePosY;
        [SerializeField]
        private bool showTooltipDesc;
        [SerializeField]
        private int tooltipDescPosX;
        [SerializeField]
        private int tooltipDescPosY;
        [SerializeField]
        private int tooltipDescSizeX;
        [SerializeField]
        private int tooltipDescSizeY;
        [SerializeField]
        private bool showTooltipAttributes;
        [SerializeField]
        private int showTooltipAttributesPosX;
        [SerializeField]
        private int showTooltipAttributesPosY;
        [SerializeField]
        private int showTooltipAttributesSizeX;
        [SerializeField]
        private int showTooltipAttributesSizeY;

        //Tooltip Objects
        [SerializeField]
        private GameObject tooltip;
        [SerializeField]
        private RectTransform tooltipRectTransform;
        [SerializeField]
        private GameObject tooltipTextName;
        [SerializeField]
        private GameObject tooltipTextDesc;
        [SerializeField]
        private GameObject tooltipImageIcon;
        [SerializeField]
        private GameObject tooltipTextAttributes;
        private string itemAttributes;

        void Start()
        {
            deactivateTooltip();
        }

#if UNITY_EDITOR
        [MenuItem("RPG/Create/Tooltip")]        //creating the menu item
        public static void menuItemCreateInventory()       //create the inventory at start
        {
            if (GameObject.FindGameObjectWithTag("Tooltip") == null)
            {
                GameObject toolTip = (GameObject)Instantiate(Resources.Load("Prefabs/Tooltip - Inventory") as GameObject);
                toolTip.GetComponent<RectTransform>().localPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                toolTip.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
                toolTip.AddComponent<Tooltip>().setImportantVariables();
            }
        }
#endif
        public void setImportantVariables()
        {
            tooltipRectTransform = this.GetComponent<RectTransform>();

            tooltipTextName = this.transform.GetChild(2).gameObject;
            tooltipTextName.SetActive(false);
            tooltipImageIcon = this.transform.GetChild(1).gameObject;
            tooltipImageIcon.SetActive(false);
            tooltipTextDesc = this.transform.GetChild(3).gameObject;
            tooltipTextDesc.SetActive(false);
            tooltipTextAttributes = this.transform.GetChild(4).gameObject;
            tooltipTextAttributes.SetActive(false);

            tooltipIconSize = 50;
            tooltipWidth = 150;
            tooltipHeight = 250;
            tooltipDescSizeX = 100;
            tooltipDescSizeY = 100;
            showTooltipAttributesSizeX = 100;
            showTooltipAttributesSizeY = 100;
        }

        public void setVariables()
        {
            tooltipBackground = transform.GetChild(0).GetComponent<Image>();
            tooltipNameText = transform.GetChild(2).GetComponent<Text>();
            tooltipDescText = transform.GetChild(3).GetComponent<Text>();
            tooltipAttributeText = transform.GetChild(4).GetComponent<Text>();
        }

        public void activateTooltip()              
        {
            tooltipTextName.SetActive(true);
            tooltipImageIcon.SetActive(true);
            tooltipTextDesc.SetActive(true);
            tooltipTextAttributes.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(true);         
            transform.GetChild(1).GetComponent<Image>().sprite = item.itemIcon;         
            transform.GetChild(2).GetComponent<Text>().text = item.itemName;            
            transform.GetChild(3).GetComponent<Text>().text = item.itemDesc;

            for (int i = 0; i < item.itemAttributes.Count; i++)
            {
                if (item.itemAttributes[i] != null)
                {
                    itemAttributes += "+ " + item.itemAttributes[i].attributeValue + " " + item.itemAttributes[i].attributeName + "\n";
                }
            }

            transform.GetChild(4).GetComponent<Text>().text = itemAttributes;
        }

        public void deactivateTooltip()             
        {
            tooltipTextName.SetActive(false);
            tooltipImageIcon.SetActive(false);
            tooltipTextDesc.SetActive(false);
            tooltipTextAttributes.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(false);

            itemAttributes = null;
        }

        public void updateTooltip()
        {
            if (!Application.isPlaying)
            {
                tooltipRectTransform.sizeDelta = new Vector2(tooltipWidth, tooltipHeight);

                if (showTooltipName)
                {
                    tooltipTextName.gameObject.SetActive(true);
                    this.transform.GetChild(2).GetComponent<RectTransform>().localPosition = new Vector3(tooltipNamePosX, tooltipNamePosY, 0);
                }
                else
                {
                    this.transform.GetChild(2).gameObject.SetActive(false);
                }

                if (showTooltipIcon)
                {
                    this.transform.GetChild(1).gameObject.SetActive(true);
                    this.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(tooltipIconPosX, tooltipIconPosY, 0);
                    this.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(tooltipIconSize, tooltipIconSize);
                }
                else
                {
                    this.transform.GetChild(1).gameObject.SetActive(false);
                }

                if (showTooltipDesc)
                {
                    this.transform.GetChild(3).gameObject.SetActive(true);
                    this.transform.GetChild(3).GetComponent<RectTransform>().localPosition = new Vector3(tooltipDescPosX, tooltipDescPosY, 0);
                    this.transform.GetChild(3).GetComponent<RectTransform>().sizeDelta = new Vector2(tooltipDescSizeX, tooltipDescSizeY);
                }
                else
                {
                    this.transform.GetChild(3).gameObject.SetActive(false);
                }
                if(showTooltipAttributes)
                {
                    this.transform.GetChild(4).gameObject.SetActive(true);
                    this.transform.GetChild(4).GetComponent<RectTransform>().localPosition = new Vector3(showTooltipAttributesPosX, showTooltipAttributesPosY, 0);
                    this.transform.GetChild(4).GetComponent<RectTransform>().sizeDelta = new Vector2(showTooltipAttributesSizeX, showTooltipAttributesSizeY);
                }
                else
                {
                    this.transform.GetChild(4).gameObject.SetActive(false);
                }
            }
        }
    }
}
