# Meteor简介
## 概述
* Meteor是什么
Meteor是一款基于Node.js智商的全栈开发框架，用来开发实时Web应用。
Meteor是一系列包的集合，即一组预先写好的独立模块。
Meteor是一个命令行工具，类似于make，rake，或Visual Studio的命令行部分。

* 为何使用Meteor
Meteor的核心思想是，在浏览器和服务器上，一切都应该以相同的方式工作。

## 七大原则
* 数据线路(Data on the Wire):Meteor从不发送HTML到客户端，服务器只传送数据，由客户端来渲染页面。
* 同一种语言(One Language):Meteor在前后端使用同一种语言JavaScript
* 双端数据库(Database Everywhere):你的数据库不仅仅存在服务器端，还存在于你的浏览器内部，你可以使用相同的代码获取数据。
* 延迟补偿(Lantency Compensation):在客户端Meteor预先取得数据来渲染页面，这使它看起来像立即从服务器获取结果一样。
* 全栈联动(Full Stack Reactivity):Meteor App都是实时App，从后台数据库到页面模型，全部都会随着数据的变化而自动更新。
* 拥抱社区(Embrace the Ecosystem):Meteor是开源框架，并和其他已有框架集成共存，比如一套代码同时编译称为Web App和iOs、Android等多平台的移动App
* 简单即使高效(Simplicity Equals Productivity):使某事看起来简单的 最佳方式就是却是让他变简单。Meteor的主要功能都有简洁优雅的API