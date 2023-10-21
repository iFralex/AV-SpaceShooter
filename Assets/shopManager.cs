using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using System;

public class shopManager : MonoBehaviour, IStoreListener
{
    public menù menu;
    public RectTransform articoli;
    public Button selezionaBt;
    public Image skinMenuIm;
    public List<Articolo> listaArticoli;
    public enum StatiPulsante { seleziona, acquista }
    public StatiPulsante stato;
    Articolo articoloSel;
    Image attivoPrimaIm;
    Color attivoPrimaCol;
    public static List<Sprite> listaSkin = new List<Sprite>();

    void Start()
    {
        for (int i = 0; i < listaArticoli.Count; i++)
            if (PlayerPrefs.HasKey(listaArticoli[i].id.ToString()))
                listaArticoli[i].acquistato = true;
        //PlayerPrefs.DeleteKey(listaArticoli[i].id.ToString());//

        ImpostaArticoli();
        Seleziona(0);
    }

    void ImpostaArticoli()
    {
        for (int i = 0; i < articoli.childCount; i++)
        {
            articoli.GetChild(i).GetChild(1).GetComponent<Image>().sprite = listaArticoli[i].skin;
            articoli.GetChild(i).GetChild(3).GetComponent<Text>().text = listaArticoli[i].nome;
            if (listaArticoli[i].acquistato)
            {
                articoli.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(0, 1, 0, .5f);
                articoli.GetChild(i).GetChild(2).GetComponent<Text>().text = "";
            }
            else
                articoli.GetChild(i).GetChild(2).GetComponent<Text>().text = listaArticoli[i].prezzo + "$";
        }
    }

    public void Seleziona(int n)
    {
        articoloSel = listaArticoli[n];
        if (articoloSel.acquistato)
            stato = StatiPulsante.seleziona;
        else
            stato = StatiPulsante.acquista;

        switch (stato)
        {
            case StatiPulsante.seleziona:
                selezionaBt.transform.GetChild(1).gameObject.SetActive(true);
                selezionaBt.transform.GetChild(0).gameObject.SetActive(false);
                selezionaBt.GetComponent<IAPButton>().enabled = false;
                selezionaBt.GetComponent<IAPButton>().productId = "";
                break;
            case StatiPulsante.acquista:
                selezionaBt.transform.GetChild(0).gameObject.SetActive(true);
                selezionaBt.transform.GetChild(1).gameObject.SetActive(false);
                selezionaBt.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = articoloSel.prezzo + " $";
                selezionaBt.GetComponent<IAPButton>().productId = articoloSel.idIAP;
                selezionaBt.GetComponent<IAPButton>().enabled = true;
                break;
        }

        if (attivoPrimaIm != null)
            attivoPrimaIm.color = attivoPrimaCol;
        attivoPrimaIm = articoli.GetChild(n).GetChild(0).GetComponent<Image>();
        attivoPrimaCol = attivoPrimaIm.color;
        attivoPrimaIm.color = new Color(1, 1, 0, .5f);
    }

    public void AssegnaSkin()
    {
        menù.skin = articoloSel.skin;
        skinMenuIm.sprite = articoloSel.skin;
    }

    public void AcquistaOSeleziona()
    {
        if (stato == StatiPulsante.seleziona)
        {
            AssegnaSkin();
            ImpostaArticoli();
            Seleziona(attivoPrimaIm.transform.parent.GetSiblingIndex());
            gameObject.SetActive(false);
        }
    }

    public void AcquistoCompletato(Product prodotto)
    {
        Debug.Log("completato: " + prodotto.definition.id);
        AssegnaSkin();
        articoloSel.acquistato = true;
        PlayerPrefs.SetInt(articoloSel.id.ToString(), 1);
        attivoPrimaCol = new Color(0, 1, 0, .5f);
        ImpostaArticoli();
        Seleziona(attivoPrimaIm.transform.parent.GetSiblingIndex());
        MostraMessaggio.mm.Messaggio(MostraMessaggio.Messaggi.successo, "Great!\nThe purchase of \"" + prodotto.metadata.localizedTitle + "\" was succesdful!");
        gameObject.SetActive(false);
    }

    public void AcquistoFallito(Product prodotto, PurchaseFailureReason causa)
    {
        Debug.Log("fallito: " + prodotto.definition.id + "  perché: " + causa);
        MostraMessaggio.mm.Messaggio(MostraMessaggio.Messaggi.errore, "OOPS, An error has occurred; the reison is: " + causa);
    }

    void IStoreListener.OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("non inizializzato");
        MostraMessaggio.mm.Messaggio(MostraMessaggio.Messaggi.errore, "Not initialized; the reison is: " + error);
    }

    void IStoreListener.OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("inizializzato");
        MostraMessaggio.mm.Messaggio(MostraMessaggio.Messaggi.avviso, "Initializzed");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        throw new NotImplementedException();
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        throw new NotImplementedException();
    }
    /*
//iap
public void InitializePurchasing()
{
// If we have already connected to Purchasing ...
if (IsInitialized())
{
  // ... we are done here.
  return;
}

// Create a builder, first passing in a suite of Unity provided stores.
var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

// Add a product to sell / restore by way of its identifier, associating the general identifier
// with its store-specific identifiers.
builder.AddProduct(kProductIDConsumable, ProductType.Consumable);
// Continue adding the non-consumable product.
builder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable);
// And finish adding the subscription product. Notice this uses store-specific IDs, illustrating
// if the Product ID was configured differently between Apple and Google stores. Also note that
// one uses the general kProductIDSubscription handle inside the game - the store-specific IDs 
// must only be referenced here. 
builder.AddProduct(kProductIDSubscription, ProductType.Subscription, new IDs(){
      { kProductNameAppleSubscription, AppleAppStore.Name },
      { kProductNameGooglePlaySubscription, GooglePlay.Name },
  });

// Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
// and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
UnityPurchasing.Initialize(this, builder);
}


private bool IsInitialized()
{
// Only say we are initialized if both the Purchasing references are set.
return m_StoreController != null && m_StoreExtensionProvider != null;
}


public void BuyConsumable()
{
// Buy the consumable product using its general identifier. Expect a response either 
// through ProcessPurchase or OnPurchaseFailed asynchronously.
BuyProductID(kProductIDConsumable);
}


public void BuyNonConsumable()
{
// Buy the non-consumable product using its general identifier. Expect a response either 
// through ProcessPurchase or OnPurchaseFailed asynchronously.
BuyProductID(kProductIDNonConsumable);
}


public void BuySubscription()
{
// Buy the subscription product using its the general identifier. Expect a response either 
// through ProcessPurchase or OnPurchaseFailed asynchronously.
// Notice how we use the general product identifier in spite of this ID being mapped to
// custom store-specific identifiers above.
BuyProductID(kProductIDSubscription);
}


void BuyProductID(string productId)
{
// If Purchasing has been initialized ...
if (IsInitialized())
{
  // ... look up the Product reference with the general product identifier and the Purchasing 
  // system's products collection.
  Product product = m_StoreController.products.WithID(productId);
  // If the look up found a product for this device's store and that product is ready to be sold ... 
  if (product != null && product.availableToPurchase)
  {
      Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
      // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
      // asynchronously.
      m_StoreController.InitiatePurchase(product);
  }
  // Otherwise ...
  else
  {
      // ... report the product look-up failure situation  
      Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
  }
}
// Otherwise ...
else
{
  // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
  // retrying initiailization.
  Debug.Log("BuyProductID FAIL. Not initialized.");
}
}


// Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
// Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
public void RestorePurchases()
{
// If Purchasing has not yet been set up ...
if (!IsInitialized())
{
  // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
  Debug.Log("RestorePurchases FAIL. Not initialized.");
  return;
}

// If we are running on an Apple device ... 
if (Application.platform == RuntimePlatform.IPhonePlayer ||
  Application.platform == RuntimePlatform.OSXPlayer)
{
  // ... begin restoring purchases
  Debug.Log("RestorePurchases started ...");

  // Fetch the Apple store-specific subsystem.
  var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
  // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
  // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
  apple.RestoreTransactions((result) => {
      // The first phase of restoration. If no more responses are received on ProcessPurchase then 
      // no purchases are available to be restored.
      Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
  });
}
// Otherwise ...
else
{
  // We are not running on an Apple device. No work is necessary to restore purchases.
  Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
}
}


//  
// --- IStoreListener
//

public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
{
// Purchasing has succeeded initializing. Collect our Purchasing references.
Debug.Log("OnInitialized: PASS");

// Overall Purchasing system, configured with products for this application.
m_StoreController = controller;
// Store specific subsystem, for accessing device-specific store features.
m_StoreExtensionProvider = extensions;
}


public void OnInitializeFailed(InitializationFailureReason error)
{
// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
}


public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
{
// A consumable product has been purchased by this user.
if (String.Equals(args.purchasedProduct.definition.id, kProductIDConsumable, StringComparison.Ordinal))
{
  Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
  // The consumable item has been successfully purchased, add 100 coins to the player's in-game score.
}
// Or ... a non-consumable product has been purchased by this user.
else if (String.Equals(args.purchasedProduct.definition.id, kProductIDNonConsumable, StringComparison.Ordinal))
{
  Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
  // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
  AssegnaSkin();
  articoloSel.acquistato = true;
  attivoPrimaCol = new Color(0, 1, 0, .5f);
  ImpostaArticoli();
  Seleziona(attivoPrimaIm.transform.parent.GetSiblingIndex());
  gameObject.SetActive(false);
}
// Or ... a subscription product has been purchased by this user.
else if (String.Equals(args.purchasedProduct.definition.id, kProductIDSubscription, StringComparison.Ordinal))
{
  Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
  // TODO: The subscription item has been successfully purchased, grant this to the player.
}
// Or ... an unknown product has been purchased by this user. Fill in additional products here....
else
{
  Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
}

// Return a flag indicating whether this product has completely been received, or if the application needs 
// to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
// saving purchased products to the cloud, and when that save is delayed. 
return PurchaseProcessingResult.Complete;
}


public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
{
// A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
// this reason with the user to guide their troubleshooting actions.
Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
}*/
}

[System.Serializable]
public class Articolo
{
    public string nome;
    public Sprite skin;
    public float prezzo;
    public bool acquistato;
    public int id;
    public string idIAP;
}