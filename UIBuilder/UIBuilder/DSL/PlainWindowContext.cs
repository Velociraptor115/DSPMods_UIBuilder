using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace DysonSphereProgram.Modding.UI;

public static partial class UIBuilderDSL
{
  public record PlainWindowContext(GameObject window) : WindowContext<PlainWindowContext>(window)
  {
    public override PlainWindowContext Context => this;
    protected override TranslucentImage panelBgCloneImg => UIBuilder.plainWindowPanelBg;
    protected override Image panelBgBorderCloneImg => UIBuilder.plainWindowPanelBgBorder;
    protected override Image panelBgDragTriggerCloneImg => UIBuilder.plainWindowPanelBgDragTrigger;
    protected override Image shadowCloneImg => UIBuilder.plainWindowShadowImg;

    public override WindowContext<PlainWindowContext> WithShadow()
    {
      base.WithShadow();
      new UIElementContext(uiElement.SelectChild("shadow")).OfSize(44, 44);
      return this;
    }
    
    public UIButton closeUIButton { get; set; }
    
    public PlainWindowContext WithCloseButton(System.Action closeCallback)
    {
      WithPanelBg();
      var panelBgObj = uiElement.SelectDescendant("panel-bg");
      if (panelBgObj.SelectChild("x") != null)
        return this;

      var closeBtnObj = 
        Create.UIElement("x")
          .CloneComponentFrom(UIBuilder.plainWindowPanelBgX)
          .ChildOf(panelBgObj)
          .WithAnchor(Anchor.TopRight)
          .OfSize(21, 21)
          .At(-13, -13)
          .uiElement
          ;
      
      var wasActive = uiElement.activeSelf;
      if (wasActive)
        uiElement.SetActive(false);
      
      var unityButton = closeBtnObj.GetOrCreateComponent<Button>();
      closeUIButton = closeBtnObj.GetOrCreateComponent<UIButton>();
      closeUIButton.CopyFrom(UIBuilder.plainWindowPanelBgCloseUIButton);

      if (closeUIButton.transitions.Length >= 1)
      {
        closeUIButton.transitions[0].target =
          closeBtnObj.GetComponent<Image>();
      }

      if (wasActive)
        uiElement.SetActive(true);
      
      unityButton.onClick.AddListener(new UnityAction(closeCallback));

      return this;
    }
  }
}