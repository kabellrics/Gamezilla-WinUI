namespace GameZilla.Services;

public interface IPageTemplateSelector
{
    void AddSkin(string skinName);
    void RemoveSkin(string skinName);
}