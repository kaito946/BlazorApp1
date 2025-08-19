using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// PageRouteModelConvention��ǉ�
builder.Services.Configure<RazorPagesOptions>(options =>
{
    var userBasePath = builder.Configuration["UserBasePath"]?.TrimEnd('/') ?? "";

    // ���ׂẴy�[�W�Ƀx�[�X�p�X��ǉ�
    options.Conventions.Add(new CustomPageRouteModelConvention(userBasePath));
});

var app = builder.Build();

app.UseRouting();
app.MapRazorPages();
app.MapBlazorHub();

app.Run();

// �J�X�^���y�[�W���[�g�K��
public class CustomPageRouteModelConvention : IPageRouteModelConvention
{
    private readonly string _basePath;

    public CustomPageRouteModelConvention(string basePath)
    {
        _basePath = basePath;
    }

    public void Apply(PageRouteModel model)
    {
        var selectorCount = model.Selectors.Count;
        for (var i = 0; i < selectorCount; i++)
        {
            var selector = model.Selectors[i];
            var template = selector.AttributeRouteModel?.Template;

            if (!string.IsNullOrEmpty(template))
            {
                // �����̃��[�g��V�����x�[�X�p�X�t���̃��[�g�ɕύX
                selector.AttributeRouteModel.Template = $"{_basePath.TrimStart('/')}/{template.TrimStart('/')}";
            }
        }
    }
}