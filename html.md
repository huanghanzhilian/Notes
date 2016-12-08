[TOC]

## 什么是HTML
html是用来描述网页的一种语言
* HTML 指的是超文本标记语言: HyperText Markup Language
* HTML 不是一种编程语言，而是一种标记语言
* 标记语言是一套标记标签 (markup tag)
* HTML 使用标记标签来描述网页
* HTML 文档包含了HTML 标签及文本内容
* HTML文档也叫做 web 页面

## HTML 标签
HTML 标记标签通常被称为 HTML 标签 (HTML tag)。
* HTML 标签是由尖括号包围的关键词，比如 <html>
* HTML 标签通常是成对出现的，比如 <b> 和 </b>
* 标签对中的第一个标签是开始标签，第二个标签是结束标签
* 开始和结束标签也被称为开放标签和闭合标签

```html
<标签>内容</标签>
```

## HTML 元素
"HTML 标签" 和 "HTML 元素" 通常都是描述同样的意思.但是严格来讲, 一个 HTML 元素包含了开始标签与结束标签

```html
<p>这是一个段落。</p>
```

## HTML版本

| 版本        | 发布时间 |
| --------- | ---- |
| HTML      | 1991 |
| HTML+     | 1993 |
| HTML 2.0  | 1995 |
| HTML 3.2  | 1997 |
| HTML 4.01 | 1999 |
| XHTML 1.0 | 2000 |
| HTML 5    | 2012 |
| XHTML     | 2013 |


## <!DOCTYPE> 声明
<!DOCTYPE>声明有助于浏览器中正确显示网页。
网络上有很多不同的文件，如果能够正确声明HTML的版本，浏览器就能正确显示网页内容。
doctype 声明是不区分大小写的，以下方式均可：
```html
<!DOCTYPE html> 
<!DOCTYPE HTML> 
<!doctype html> 
<!Doctype Html>
```

## 通用声明
```html
<!--html5-->
<!DOCTYPE html>
<!--html4.01-->
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN"
"http://www.w3.org/TR/html4/loose.dtd">
<!--XHTML 1.0-->
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
```

> DOCTYPE标签定义及使用说明
> <!DOCTYPE> 声明位于文档中的最前面的位置，处于 <html> 标签之前。
> <!DOCTYPE> 声明不是一个 HTML 标签；它是用来告知 Web 浏览器页面使用了哪种 HTML 版本。
> 在 HTML 4.01 中，<!DOCTYPE> 声明需引用 DTD （文档类型声明），因为 HTML 4.01 是基于 SGML （Standard Generalized Markup Language 标准通用标记语言）。DTD 指定了标记语言的规则，确保了浏览器能够正确的渲染内容。
> HTML5 不是基于 SGML，因此不要求引用 DTD。

## 中文编码
目前在大部分浏览器中，直接输出中文会出现中文乱码的情况，这时候我们就需要在头部将字符声明为 UTF-8。
```html
<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<title>页面标题</title>
</head>
```

## HTML元素语法
* HTML 元素以开始标签起始
* HTML 元素以结束标签终止
* 元素的内容是开始标签与结束标签之间的内容
* 某些 HTML 元素具有空内容（empty content）
* 空元素在开始标签中进行关闭（以开始标签的结束而结束）
* 大多数 HTML 元素可拥有属性

## HTML空元素
没有内容的 HTML 元素被称为空元素。空元素是在开始标签中关闭的。
<br> 就是没有关闭标签的空元素（<br> 标签定义换行）。
在 XHTML、XML 以及未来版本的 HTML 中，所有元素都必须被关闭。
在开始标签中添加斜杠，比如 <br />，是关闭空元素的正确方法，HTML、XHTML 和 XML 都接受这种方式。
即使 <br> 在所有浏览器中都是有效的，但使用 <br /> 其实是更长远的保障。

## HTML 属性
* HTML 元素可以设置属性
* 属性可以在元素中添加附加信息
* 属性一般描述于开始标签
* 属性总是以名称/值对的形式出现，比如：name="value"。

## HTML文本格式化

| 标签       | 描述     |
| -------- | ------ |
| <b>      | 定义粗体文本 |
| <em>     | 定义着重文字 |
| <i>      | 定义斜体字  |
| <small>  | 定义小号字  |
| <strong> | 定义加重语气 |
| <sub>    | 定义下标字  |
| <sup>    | 定义上标字  |
| <ins>    | 定义插入字  |
| <del>    | 定义删除字  |

## HTML "计算机输出" 标签

| 标签     | 描述        |
| ------ | --------- |
| <code> | 定义计算机代码   |
| <kbd>  | 定义键盘码     |
| <samp> | 定义计算机代码样本 |
| <var>  | 定义变量      |
| <pre>  | 定义预格式文本   |

## HTML 引文, 引用, 及标签定义

| 标签           | 描述       |
| ------------ | -------- |
| <abbr>       | 定义缩写     |
| <address>    | 定义地址     |
| <bdo>        | 定义文字方向   |
| <blockquote> | 定义长的引用   |
| <q>          | 定义短的引用   |
| <cite>       | 定义引用、引证  |
| <dfn>        | 定义一个定义项目 |

## HTML <head> 元素
<head> 元素包含了所有的头部标签元素。在 <head>元素中你可以插入脚本（scripts）, 样式文件（CSS），及各种meta信息。
可以添加在头部区域的元素标签为: <title>, <style>, <meta>, <link>, <script>, <noscript>, and <base>.

## HTML <title> 元素
<title> 标签定义了不同文档的标题。
<title> 在 HTML/XHTML 文档中是必须的。
<title> 元素:
* 定义了浏览器工具栏的标题
* 当网页添加到收藏夹时，显示在收藏夹中的标题
* 显示在搜索引擎结果页面的标题

## HTML <base> 元素
<base> 标签描述了基本的链接地址/链接目标，该标签作为HTML文档中所有的链接标签的默认链接

## HTML <link> 元素
<link> 标签定义了文档与外部资源之间的关系。
<link> 标签通常用于链接到样式表:
```html
<head>
<link rel="stylesheet" type="text/css" href="mystyle.css">
</head>
```

## HTML <style> 元素
<style> 标签定义了HTML文档的样式文件引用地址.
在<style> 元素中你需要指定样式文件来渲染HTML文档

## HTML <meta> 元素
meta标签描述了一些基本的元数据。
<meta> 标签提供了元数据.元数据也不显示在页面上，但会被浏览器解析。
META元素通常用于指定网页的描述，关键词，文件的最后修改时间，作者，和其他元数据。
元数据可以使用于浏览器（如何显示内容或重新加载页面），搜索引擎（关键词），或其他Web服务。
<meta>一般放置于 <head>区域
```html
<!--为搜索引擎定义关键词-->
<meta name="keywords" content="HTML, CSS, XML, XHTML, JavaScript">
<!--为网页定义描述内容-->
<meta name="description" content="Free Web tutorials on HTML and CSS">
<!--定义网页作者-->
<meta name="author" content="Hege Refsnes">
<!--每30秒中刷新当前页面-->
<meta http-equiv="refresh" content="30">
```

## HTML 自定义列表
自定义列表不仅仅是一列项目，而是项目及其注释的组合。
自定义列表以 <dl> 标签开始。每个自定义列表项以 <dt> 开始。每个自定义列表项的定义以 <dd> 开始。
```html
<dl>
<dt>Coffee</dt>
<dd>- black hot drink</dd>
<dt>Milk</dt>
<dd>- white cold drink</dd>
</dl>
```

## HTML<noscript> 标签
<noscript> 标签提供无法使用脚本时的替代内容，比方在浏览器禁用脚本时，或浏览器不支持客户端脚本时。
<noscript>元素可包含普通 HTML 页面的 body 元素中能够找到的所有元素。
只有在浏览器不支持脚本或者禁用脚本时，才会显示 <noscript> 元素中的内容

## 音标符

| 音标符  | 字符   | Construct | 输出结果 |
| ---- | ---- | --------- | ---- |
| ̀    | a    | a`&#768;` | à   |
| ́    | a    | a`&#769;` | á   |
| ̂    | a    | a`&#770;` | â   |
| ̃    | a    | a`&#771;` | ã   |

## HTML字符实体

| 显示结果 | 描述   | 实体名称 | 实体编号 |
| ---- | ---- | ---- | ---- |
|      |   空格   |   &nbsp;   |  `&#160;`    |
|   <   |   	小于号   |  &lt;    |    `&#60;`  |
| >     | 大于号     | &gt;     | `&#62;`     |
|   &   |和号      |   &amp;   |  `&#38;`    |
|   "   |   引号   |   &quot;   |  `&#34;`    |
|  '    |   撇号    |   &apos; (IE不支持)   |    `&#39;`  |
|   ￠   |   分   |   &cent;   |    `&#162;`  |
|   £   |   镑   |    &pound;  |   `&#163;`   |
|   ¥   |  日元    |   &yen;   |   `&#165;`   |
|  €    |   欧元   |   &euro;   |   `&#8364;`   |
|   §   |   小节   |    &sect;  |   `&#167;`   |
|   ©   |   版权   |   &copy;   |   `&#169;`   |
|   ®   |  注册商标    |   &reg;   |   `&#174;`   |
|   ™   |   商标   |  &trade;    |  `&#8482;`    |
|   ×   |   乘号   |   &times;   |   `&#215;`   |
|   ÷   |   除号   |   &divide;   |   `&#247;`   |