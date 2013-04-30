<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UserGuide.aspx.cs" Inherits="UserGuide" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">

h4
	{border-style: none; border-color: inherit; border-width: medium; margin-top:15.0pt;
	    margin-right:0cm;
	margin-bottom:0cm;
	    margin-left:0cm;
	    margin-bottom:.0001pt;
	    line-height:115%;
	    padding:0cm;
	    font-size:11.0pt;
	    font-family:"Calibri","sans-serif";
	    color:#365F91;
	    text-transform:uppercase;
	    letter-spacing:.5pt;
	    font-weight:normal; }
 p.MsoNormal
	{margin: 10.0pt 0cm; line-height:115%;
	    font-size:10.0pt;
	    font-family:"Calibri","sans-serif";
	}
ul
	{margin-bottom:0cm;}
 li.MsoNormal
	{margin: 10.0pt 0cm; line-height:115%;
	    font-size:10.0pt;
	    font-family:"Calibri","sans-serif";
	}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <div style="height:30px;">更新日期：2013-01-11</div>
    <div style="mso-element:para-border-div;border-top:dotted #4F81BD 1.0pt;
mso-border-top-themecolor:accent1;border-left:dotted #4F81BD 1.0pt;mso-border-left-themecolor:
accent1;border-bottom:none;border-right:none;mso-border-top-alt:dotted #4F81BD .75pt;
mso-border-top-themecolor:accent1;mso-border-left-alt:dotted #4F81BD .75pt;
mso-border-left-themecolor:accent1;padding:2.0pt 0cm 0cm 2.0pt">
        <h4><span style="font-family:宋体;mso-ascii-font-family:Calibri;mso-ascii-theme-font:
minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:minor-fareast;
mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">业绩查询</span><span lang="EN-US"><o:p></o:p></span></h4>
    </div>
    <p class="MsoNormal">
        <span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">权限要求：一般用户只能查看自己及其自己下属的业绩，公司助理和公司经理可以查看本公司全部。</span><span lang="EN-US"><o:p></o:p></span></p>
    <p class="MsoNormal">
        <span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">默认以详细方式显示，可以点击“汇总信息”按业务员进行统计。</span><span lang="EN-US"><o:p></o:p></span></p>
    <div style="mso-element:para-border-div;border-top:dotted #4F81BD 1.0pt;
mso-border-top-themecolor:accent1;border-left:dotted #4F81BD 1.0pt;mso-border-left-themecolor:
accent1;border-bottom:none;border-right:none;mso-border-top-alt:dotted #4F81BD .75pt;
mso-border-top-themecolor:accent1;mso-border-left-alt:dotted #4F81BD .75pt;
mso-border-left-themecolor:accent1;padding:2.0pt 0cm 0cm 2.0pt">
        <h4><span style="font-family:宋体;mso-ascii-font-family:Calibri;mso-ascii-theme-font:
minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:minor-fareast;
mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">业绩添加</span><span lang="EN-US"><o:p></o:p></span></h4>
    </div>
    <p class="MsoNormal">
        <span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">权限要求：公司经理或者公司助理</span><span lang="EN-US"><o:p></o:p></span></p>
    <p class="MsoNormal">
        <span lang="EN-US">
        <o:p>
        &nbsp;</o:p></span></p>
    <p class="MsoNormal">
        <span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">通过上传</span><span lang="EN-US">EXCEL</span><span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">格式文件导入，导入时需要选择一个日期，如果该日期以及存在业绩。会得到相关提示“业绩已存在，请删除旧数据再导入”。</span><span lang="EN-US"><o:p></o:p></span></p>
    <div style="mso-element:para-border-div;border-top:dotted #4F81BD 1.0pt;
mso-border-top-themecolor:accent1;border-left:dotted #4F81BD 1.0pt;mso-border-left-themecolor:
accent1;border-bottom:none;border-right:none;mso-border-top-alt:dotted #4F81BD .75pt;
mso-border-top-themecolor:accent1;mso-border-left-alt:dotted #4F81BD .75pt;
mso-border-left-themecolor:accent1;padding:2.0pt 0cm 0cm 2.0pt">
        <h4><span style="font-family:宋体;mso-ascii-font-family:Calibri;mso-ascii-theme-font:
minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:minor-fareast;
mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">客户管理</span><span lang="EN-US"><o:p></o:p></span></h4>
    </div>
    <p class="MsoNormal">
        <span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">权限要求</span><span lang="EN-US">-</span><span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">查询：一般用户只能查看自己及其自己下属的客户，公司助理和公司经理可以查看本公司全部。</span><span lang="EN-US"><o:p></o:p></span></p>
    <p class="MsoNormal">
        <span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">母公司的员工可以查询子公司的公海客户，子公司员工只能查询自己所属公司的公海客户。</span><span lang="EN-US"><o:p></o:p></span></p>
    <p class="MsoNormal">
        <span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">权限要求</span><span lang="EN-US">-</span><span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">编辑：公司经理以上人员才可以编辑客户所属业务员。</span><span lang="EN-US"><o:p></o:p></span></p>
    <p class="MsoNormal">
        <span lang="EN-US" style="mso-no-proof:yes"><!--[if gte vml 1]>
        <v:shapetype
 id="_x0000_t75" coordsize="21600,21600" o:spt="75" o:preferrelative="t"
 path="m@4@5l@4@11@9@11@9@5xe" filled="f" stroked="f">
        <v:stroke joinstyle="miter" xmlns:v="urn:schemas-microsoft-com:vml"/>
        <v:formulas>
        <v:f eqn="if lineDrawn pixelLineWidth 0" xmlns:v="urn:schemas-microsoft-com:vml"/>
        <v:f eqn="sum @0 1 0" xmlns:v="urn:schemas-microsoft-com:vml"/>
        <v:f eqn="sum 0 0 @1" xmlns:v="urn:schemas-microsoft-com:vml"/>
        <v:f eqn="prod @2 1 2" xmlns:v="urn:schemas-microsoft-com:vml"/>
        <v:f eqn="prod @3 21600 pixelWidth" xmlns:v="urn:schemas-microsoft-com:vml"/>
        <v:f eqn="prod @3 21600 pixelHeight" xmlns:v="urn:schemas-microsoft-com:vml"/>
        <v:f eqn="sum @0 0 1" xmlns:v="urn:schemas-microsoft-com:vml"/>
        <v:f eqn="prod @6 1 2" xmlns:v="urn:schemas-microsoft-com:vml"/>
        <v:f eqn="prod @7 21600 pixelWidth" xmlns:v="urn:schemas-microsoft-com:vml"/>
        <v:f eqn="sum @8 21600 0" xmlns:v="urn:schemas-microsoft-com:vml"/>
        <v:f eqn="prod @7 21600 pixelHeight" xmlns:v="urn:schemas-microsoft-com:vml"/>
        <v:f eqn="sum @10 21600 0" xmlns:v="urn:schemas-microsoft-com:vml"/>
        </v:formulas>
        <v:path o:extrusionok="f" gradientshapeok="t" o:connecttype="rect" xmlns:v="urn:schemas-microsoft-com:vml"/>
        <o:lock v:ext="edit" aspectratio="t" xmlns:o="urn:schemas-microsoft-com:office:office"/>
        </v:shapetype>
        <v:shape id="图片_x0020_2" o:spid="_x0000_i1030" type="#_x0000_t75"
 style='width:414.75pt;height:185.25pt;visibility:visible;mso-wrap-style:square'>
        <v:imagedata src="Images/content/clip_image001.png"
  o:title="" xmlns:v="urn:schemas-microsoft-com:vml"/>
        </v:shape>
        <![endif]--><![if !vml]>
        <img height="247" src="Images/content/clip_image002.jpg" v:shapes="图片_x0020_2" width="553" /><![endif]></span><span lang="EN-US"><o:p></o:p></span></p>
    <p class="MsoNormal">
        <span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">交易商代码不能重复，</span><span lang="EN-US">[</span><span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">联系电话</span><span lang="EN-US">]+[</span><span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">客户类型</span><span lang="EN-US">]</span><span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">不能重复。</span><span lang="EN-US"><o:p></o:p></span></p>
    <p class="MsoNormal">
        <span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">【公海查询】和【客户查询】实际上为同一个页面。</span><span lang="EN-US"><o:p></o:p></span></p>
    <div style="mso-element:para-border-div;border-top:dotted #4F81BD 1.0pt;
mso-border-top-themecolor:accent1;border-left:dotted #4F81BD 1.0pt;mso-border-left-themecolor:
accent1;border-bottom:none;border-right:none;mso-border-top-alt:dotted #4F81BD .75pt;
mso-border-top-themecolor:accent1;mso-border-left-alt:dotted #4F81BD .75pt;
mso-border-left-themecolor:accent1;padding:2.0pt 0cm 0cm 2.0pt">
        <h4><span style="font-family:宋体;mso-ascii-font-family:Calibri;mso-ascii-theme-font:
minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:minor-fareast;
mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">用户管理</span><span lang="EN-US"><o:p></o:p></span></h4>
    </div>
    <p class="MsoNormal">
        <span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">权限要求：部门主管以上的权限</span><span lang="EN-US"><o:p></o:p></span></p>
    <p class="MsoNormal">
        <span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">上级临到层级设定：如果是公司的</span><span lang="EN-US">BOSS, </span><span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">角色请选择“公司经理”，上级领导请选择无。</span><span lang="EN-US"><o:p></o:p></span></p>
    <p class="MsoNormal">
        <span lang="EN-US" style="mso-no-proof:yes"><!--[if gte vml 1]>
        <v:shape
 id="图片_x0020_4" o:spid="_x0000_i1029" type="#_x0000_t75" style='width:291.75pt;
 height:204.75pt;visibility:visible;mso-wrap-style:square'>
        <v:imagedata src="Images/content/clip_image003.png"
  o:title="" xmlns:v="urn:schemas-microsoft-com:vml"/>
        </v:shape>
        <![endif]--><![if !vml]>
        <img height="273" src="Images/content/clip_image003.png" v:shapes="图片_x0020_4" width="389" /><![endif]></span><span lang="EN-US"><o:p></o:p></span></p>
    <p class="MsoNormal">
        <span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">目前系统一个员工只能有一个角色，暂时不支持既是公司助理又是销售的情况。</span><span lang="EN-US"><o:p></o:p></span></p>
    <p class="MsoNormal">
        <span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">目前系统支持很多级的员工层次，未作限制。在用户查询页面，点击按钮【人力资源关系图】，可以查看当前登录用户的所有下属。</span><span lang="EN-US"><o:p></o:p></span></p>
    <p class="MsoNormal">
        <span lang="EN-US" style="mso-no-proof:yes"><!--[if gte vml 1]>
        <v:shape
 id="图片_x0020_5" o:spid="_x0000_i1028" type="#_x0000_t75" style='width:153.75pt;
 height:138pt;visibility:visible;mso-wrap-style:square'>
        <v:imagedata src="Images/content/clip_image004.png"
  o:title="" xmlns:v="urn:schemas-microsoft-com:vml"/>
        </v:shape>
        <![endif]--><![if !vml]>
        <img height="184" src="Images/content/clip_image004.png" v:shapes="图片_x0020_5" width="205" /><![endif]></span><span lang="EN-US"><o:p></o:p></span></p>
    <div style="mso-element:para-border-div;border-top:dotted #4F81BD 1.0pt;
mso-border-top-themecolor:accent1;border-left:dotted #4F81BD 1.0pt;mso-border-left-themecolor:
accent1;border-bottom:none;border-right:none;mso-border-top-alt:dotted #4F81BD .75pt;
mso-border-top-themecolor:accent1;mso-border-left-alt:dotted #4F81BD .75pt;
mso-border-left-themecolor:accent1;padding:2.0pt 0cm 0cm 2.0pt">
        <h4><span style="font-family:宋体;mso-ascii-font-family:Calibri;mso-ascii-theme-font:
minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:minor-fareast;
mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">用户删除</span><span lang="EN-US"><o:p></o:p></span></h4>
    </div>
    <p class="MsoNormal">
        <span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">权限要求：必须是经理级别才具有删除权限</span><span lang="EN-US"><o:p></o:p></span></p>
    <p class="MsoNormal">
        <span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">必须先删除下属或者将下属分配给其它公司领导</span><span lang="EN-US"><o:p></o:p></span></p>
    <p class="MsoNormal">
        <span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">对应的业绩、客户关联将会取消，变成公海。</span><span lang="EN-US"><o:p></o:p></span></p>
    <p class="MsoNormal">
        <span lang="EN-US">
        <o:p>
        &nbsp;</o:p></span></p>
    <div style="mso-element:para-border-div;border-top:dotted #4F81BD 1.0pt;
mso-border-top-themecolor:accent1;border-left:dotted #4F81BD 1.0pt;mso-border-left-themecolor:
accent1;border-bottom:none;border-right:none;mso-border-top-alt:dotted #4F81BD .75pt;
mso-border-top-themecolor:accent1;mso-border-left-alt:dotted #4F81BD .75pt;
mso-border-left-themecolor:accent1;padding:2.0pt 0cm 0cm 2.0pt">
        <h4><span style="font-family:宋体;mso-ascii-font-family:Calibri;mso-ascii-theme-font:
minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:minor-fareast;
mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">公司管理</span><span lang="EN-US"><o:p></o:p></span></h4>
    </div>
    <p class="MsoNormal">
        <span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">权限要求：只有管理员才可以进入公司管理页面编辑子公司。</span><span lang="EN-US"><o:p></o:p></span></p>
    <p class="MsoNormal">
        <span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">子公司代码必须以母公司的代码作为前缀，例如：</span><span lang="EN-US"><o:p></o:p></span></p>
    <p class="MsoNormal">
        <span lang="EN-US" style="mso-no-proof:yes"><!--[if gte vml 1]>
        <v:shape
 id="图片_x0020_3" o:spid="_x0000_i1027" type="#_x0000_t75" style='width:193.5pt;
 height:64.5pt;visibility:visible;mso-wrap-style:square'>
        <v:imagedata src="Images/content/clip_image005.png"
  o:title="" xmlns:v="urn:schemas-microsoft-com:vml"/>
        </v:shape>
        <![endif]--><![if !vml]>
        <img height="86" src="Images/content/clip_image005.png" v:shapes="图片_x0020_3" width="258" /><![endif]></span><span lang="EN-US"><o:p></o:p></span></p>
    <div style="mso-element:para-border-div;border-top:dotted #4F81BD 1.0pt;
mso-border-top-themecolor:accent1;border-left:dotted #4F81BD 1.0pt;mso-border-left-themecolor:
accent1;border-bottom:none;border-right:none;mso-border-top-alt:dotted #4F81BD .75pt;
mso-border-top-themecolor:accent1;mso-border-left-alt:dotted #4F81BD .75pt;
mso-border-left-themecolor:accent1;padding:2.0pt 0cm 0cm 2.0pt">
        <h4><span style="font-family:宋体;mso-ascii-font-family:Calibri;mso-ascii-theme-font:
minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:minor-fareast;
mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">公司切换</span><span lang="EN-US"><o:p></o:p></span></h4>
    </div>
    <p class="MsoNormal">
        <span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">如果想查看子公司的信息，需要在【控制面板】中选择其它公司进行切换。</span><span lang="EN-US"><o:p></o:p></span></p>
    <p class="MsoNormal">
        <span lang="EN-US" style="mso-no-proof:yes"><!--[if gte vml 1]>
        <v:shape
 id="图片_x0020_1" o:spid="_x0000_i1026" type="#_x0000_t75" style='width:245.25pt;
 height:72.75pt;visibility:visible;mso-wrap-style:square'>
        <v:imagedata src="Images/content/clip_image006.png"
  o:title="" xmlns:v="urn:schemas-microsoft-com:vml"/>
        </v:shape>
        <![endif]--><![if !vml]>
        <img height="97" src="Images/content/clip_image006.png" v:shapes="图片_x0020_1" width="327" /><![endif]></span><span lang="EN-US"><o:p></o:p></span></p>
    <p class="MsoNormal">
        <span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">切换到子公司后如果需要查看另外一个公司的数据，需要重新登录再进行公司切换操作。</span><span lang="EN-US"><o:p></o:p></span></p>
    <div style="mso-element:para-border-div;border-top:dotted #4F81BD 1.0pt;
mso-border-top-themecolor:accent1;border-left:dotted #4F81BD 1.0pt;mso-border-left-themecolor:
accent1;border-bottom:none;border-right:none;mso-border-top-alt:dotted #4F81BD .75pt;
mso-border-top-themecolor:accent1;mso-border-left-alt:dotted #4F81BD .75pt;
mso-border-left-themecolor:accent1;padding:2.0pt 0cm 0cm 2.0pt">
        <h4><span style="font-family:宋体;mso-ascii-font-family:Calibri;mso-ascii-theme-font:
minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:minor-fareast;
mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">自动公海</span><span lang="EN-US"><o:p></o:p></span></h4>
    </div>
    <p class="MsoNormal">
        <span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">权限要求：公司经理</span><span lang="EN-US"><o:p></o:p></span></p>
    <p class="MsoNormal" style="margin:0cm;margin-bottom:.0001pt;line-height:normal">
        <span style="font-family:宋体;mso-ascii-font-family:Simsun;mso-hansi-font-family:Simsun;
mso-bidi-font-family:宋体;color:black">公海条件</span><span lang="EN-US" style="font-size:12.0pt;font-family:宋体;mso-bidi-font-family:宋体"><o:p></o:p></span></p>
    <ul type="disc">
        <li class="MsoNormal" style="color:black;mso-margin-top-alt:auto;mso-margin-bottom-alt:
     auto;line-height:normal;mso-list:l0 level1 lfo1;tab-stops:list 36.0pt"><span style="font-family:宋体;mso-ascii-font-family:Simsun;mso-hansi-font-family:
     Simsun;mso-bidi-font-family:宋体">开户</span><span lang="EN-US" style="font-family:&quot;Simsun&quot;,&quot;serif&quot;;mso-fareast-font-family:宋体;mso-bidi-font-family:
     宋体">30</span><span style="font-family:宋体;mso-ascii-font-family:Simsun;
     mso-hansi-font-family:Simsun;mso-bidi-font-family:宋体">天都没有交易记录</span><span lang="EN-US" style="font-family:&quot;Simsun&quot;,&quot;serif&quot;;mso-fareast-font-family:
     宋体;mso-bidi-font-family:宋体"><o:p></o:p></span></li>
        <li class="MsoNormal" style="color:black;mso-margin-top-alt:auto;mso-margin-bottom-alt:
     auto;line-height:normal;mso-list:l0 level1 lfo1;tab-stops:list 36.0pt"><span style="font-family:宋体;mso-ascii-font-family:Simsun;mso-hansi-font-family:
     Simsun;mso-bidi-font-family:宋体">出现过交易记录但是</span><span lang="EN-US" style="font-family:&quot;Simsun&quot;,&quot;serif&quot;;mso-fareast-font-family:宋体;mso-bidi-font-family:
     宋体">90</span><span style="font-family:宋体;mso-ascii-font-family:Simsun;
     mso-hansi-font-family:Simsun;mso-bidi-font-family:宋体">天内都无新记录</span><span lang="EN-US" style="font-family:&quot;Simsun&quot;,&quot;serif&quot;;mso-fareast-font-family:
     宋体;mso-bidi-font-family:宋体"><o:p></o:p></span></li>
        <li class="MsoNormal" style="color:black;mso-margin-top-alt:auto;mso-margin-bottom-alt:
     auto;line-height:normal;mso-list:l0 level1 lfo1;tab-stops:list 36.0pt"><span style="font-family:宋体;mso-ascii-font-family:Simsun;mso-hansi-font-family:
     Simsun;mso-bidi-font-family:宋体">或者</span><span lang="EN-US" style="font-family:&quot;Simsun&quot;,&quot;serif&quot;;mso-fareast-font-family:宋体;mso-bidi-font-family:
     宋体">7</span><span style="font-family:宋体;mso-ascii-font-family:Simsun;
     mso-hansi-font-family:Simsun;mso-bidi-font-family:宋体">天没有更改备注</span><span lang="EN-US" style="font-family:&quot;Simsun&quot;,&quot;serif&quot;;mso-fareast-font-family:
     宋体;mso-bidi-font-family:宋体"><o:p></o:p></span></li>
    </ul>
    <p class="MsoNormal">
        <span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">考虑到还没有天天导入业绩</span><span lang="EN-US">, </span><span style="font-family:宋体;mso-ascii-font-family:Calibri;
mso-ascii-theme-font:minor-latin;mso-fareast-font-family:宋体;mso-fareast-theme-font:
minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin">把客户分配给员工后又会被系统自动公海。现在自动公海需要经理手动点一下按钮执行。</span><span lang="EN-US"><o:p></o:p></span></p>
    <p class="MsoNormal">
        <span lang="EN-US" style="mso-no-proof:yes"><!--[if gte vml 1]>
        <v:shape
 id="图片_x0020_6" o:spid="_x0000_i1025" type="#_x0000_t75" style='width:370.5pt;
 height:150.75pt;visibility:visible;mso-wrap-style:square'>
        <v:imagedata src="Images/content/clip_image007.png"
  o:title="" xmlns:v="urn:schemas-microsoft-com:vml"/>
        </v:shape>
        <![endif]--><![if !vml]>
        <img height="201" src="Images/content/clip_image007.png" v:shapes="图片_x0020_6" width="494" /><![endif]></span><span lang="EN-US"><o:p></o:p></span></p>
</asp:Content>

