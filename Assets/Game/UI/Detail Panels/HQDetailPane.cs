using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HQDetailPane : MonoBehaviour
{

    public Button buyDerrickButton, buyRigButton, buyTruckButton, buyTankerButton;
    public TextMeshProUGUI buyDerrickButtonText, buyRigButtonText, buyTruckButtonText, buyTankerButtonText;


    public Button improvePumpSpeedButton, improveOilMoveSpeedButton;
    public TextMeshProUGUI improvePumpSpeedButtonText, improveOilMoveSpeedButtonText;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        ResourceManager rm = GameObject.FindObjectOfType<ResourceManager>();
        
        float priceForDerrick = rm.PriceToBuyOilDerrick();
        if (rm.MaximumOilDerricks < 7) {
            buyDerrickButton.interactable = rm.CurrentMoney >= priceForDerrick;
            buyDerrickButtonText.SetText("$" + priceForDerrick.ToString("N2"));
        } else {
            buyDerrickButton.interactable = false;
            buyDerrickButtonText.SetText("Out of Stock");
        }
        

        float priceForRig = rm.PriceToBuyOilRig();
        if (rm.MaximumOilRigs < 17) {
            buyRigButton.interactable = rm.CurrentMoney >= priceForRig;
            buyRigButtonText.SetText("$" + priceForRig.ToString("N2"));
        } else {
            buyRigButton.interactable = false;
            buyRigButtonText.SetText("Out of Stock");
        }

        float priceForTruck = rm.PriceToBuyOilTruck();
        if (rm.MaximumOilTrucks < 999) {
            buyTruckButton.interactable = rm.CurrentMoney >= priceForTruck;
            buyTruckButtonText.SetText("$" + priceForTruck.ToString("N2"));
        } else {
            buyTruckButton.interactable = false;
            buyTruckButtonText.SetText("Out of Stock");
        }

        float priceForTanker = rm.PriceToBuyOilTanker();
        if (rm.MaximumOilTrucks < 999) {
            buyTankerButton.interactable = rm.CurrentMoney >= priceForTanker;
            buyTankerButtonText.SetText("$" + priceForTanker.ToString("N2"));
        } else {
            buyTankerButton.interactable = false;
            buyTankerButtonText.SetText("Out of Stock");
        }

    }

    public void BuyDerrick() {
        ResourceManager rm = GameObject.FindObjectOfType<ResourceManager>();
        rm.BuyDerrick();
    }

    public void BuyRig() {
        ResourceManager rm = GameObject.FindObjectOfType<ResourceManager>();
        rm.BuyRig();
    }

    public void BuyTruck() {
        ResourceManager rm = GameObject.FindObjectOfType<ResourceManager>();
        rm.BuyTruck();
    }

    public void BuyTanker() {
        ResourceManager rm = GameObject.FindObjectOfType<ResourceManager>();
        rm.BuyTanker();
    }
}
