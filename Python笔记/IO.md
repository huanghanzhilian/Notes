# IO编程
## 文件读写
### 读文件
要以读文件的模式打开一个文件对象，使用Python内置的`open()`函数，传入文件名和标示符：`f = open('/Users/michael/test.txt', 'r')`。标示符`'r'`表示读。
如果文件不存在，`open()`函数就会抛出一个`IOError`的错误，
文件读取成功之后调用`read()`方法，可以一次性读取文件的全部内容，Python把内容读到内存，用一个`str`对象表示。
最后一步是调用`close()`方法关闭文件。文件使用完毕后必须关闭，因为文件对象会占用操作系统的资源，并且操作系统同一时间能打开的文件数量也是有限的
由于文件读写时都有可能产生IOError，一旦出错，后面的f.close()就不会调用。所以，为了保证无论是否出错都能正确地关闭文件，我们可以使用try ... finally来实现：
```python
try:
    f = open('/path/to/file','r')
    print(f.read())
finally:
    if f:
        f.close()
```
Python引入了with语句来自动帮我们调用close()方法：
```python 
with open('/path/to/file', 'r') as f:
    print(f.read())
```
调用read()会一次性读取文件的全部内容，如果文件有10G，内存就爆了，所以，要保险起见，可以反复调用read(size)方法，每次最多读取size个字节的内容。另外，调用readline()可以每次读取一行内容，调用readlines()一次读取所有内容并按行返回list。
> file-like Object
> 像open()函数返回的这种有个read()方法的对象，在Python中统称为file-like Object。除了file外，还可以是内存的字节流，网络流，自定义流等等。file-like Object不要求从特定类继承，只要写个read()方法就行。
> StringIO就是在内存中创建的file-like Object，常用作临时缓冲。

### 二进制文件
要读取二进制文件，比如图片、视频等等，用'rb'模式打开文件即可：
```python
f = open('/Users/michael/test.jpg', 'rb')
f.read()
#b'\xff\xd8\xff\xe1\x00\x18Exif\x00\x00...' # 十六进制表示的字节
```
### 字符编码
要读取非UTF-8编码的文本文件，需要给open()函数传入`encoding`参数，例如，读取GBK编码的文件：
```python
f = open('/Users/michael/gbk.txt', 'r', encoding='gbk')
```
### 写文件
写文件和读文件是一样的，唯一区别是调用open()函数时，传入标识符'w'或者'wb'表示写文本文件或写二进制文件：
```python
with open('/Users/michael/test.txt', 'w') as f:
    f.write('Hello, world!')
```
要写入特定编码的文本文件，请给open()函数传入encoding参数，将字符串自动转换成指定编码。
## StringIO和BytesIO
### StringIO
很多时候，数据读写不一定是文件，也可以在内存中读写。StringIO顾名思义就是在内存中读写str。要把str写入StringIO，我们需要先创建一个StringIO，然后，像文件一样写入即可：
```python
from io import StringIO
f = StringIO()
f.write('hello')
f.write(' ')
f.write('world!')
print(f.getvalue())
# hello world!
```
`getvalue()`方法用于获得写入后的str
要读取StringIO，可以用一个str初始化StringIO，然后，像读文件一样读取：
```python
from io import StringIO
f = StringIO('Hello!\nHi!\nGoodbye!')
while True:
    s = f.readline()
    if s == '':
        break
    print(s.strip())
```
### BytesIO
StringIO操作的只能是str，如果要操作二进制数据，就需要使用BytesIO。
```python
from io import BytesIO
f = BytesIO()
f.write('中文'.encode('utf-8'))
print(f.getvalue())
# b'\xe4\xb8\xad\xe6\x96\x87' #写入的不是str，而是经过UTF-8编码的bytes。

f = BytesIO(b'\xe4\xb8\xad\xe6\x96\x87')
f.read()
# b'\xe4\xb8\xad\xe6\x96\x87'
```
## 操作文件和目录
Python内置的os模块也可以直接调用操作系统提供的接口函数。
```python
import os
os.name # 操作系统类型
# 如果是posix，说明系统是Linux、Unix或Mac OS X，如果是nt，就是Windows系统。
```
### 环境变量
在操作系统中定义的环境变量，全部保存在`os.environ`这个变量中
```python
os.environ.get('PATH')
'''
environ({'VERSIONER_PYTHON_PREFER_32_BIT': 'no', 'TERM_PROGRAM_VERSION': '326', 'LOGNAME': 'michael', 'USER': 'michael', 'PATH': '/usr/bin:/bin:/usr/sbin:/sbin:/usr/local/bin:/opt/X11/bin:/usr/local/mysql/bin', ...})
'''
```
要获取某个环境变量的值，可以调用`os.environ.get('key')`
```python
os.environ.get('PATH')
# '/usr/bin:/bin:/usr/sbin:/sbin:/usr/local/bin:/opt/X11/bin:/usr/local/mysql/bin'
```
### 操作文件和目录
操作文件和目录的函数一部分放在os模块中，一部分放在os.path模块中，这一点要注意一下。查看、创建和删除目录可以这么调用：
```python
# 查看当前目录的绝对路径:
os.path.abspath('.')
# '/Users/michael'
# 在某个目录下创建一个新目录，首先把新目录的完整路径表示出来:
os.path.join('/Users/michael', 'testdir')
# '/Users/michael/testdir'
# 然后创建一个目录:
os.mkdir('/Users/michael/testdir')
# 删掉一个目录:
os.rmdir('/Users/michael/testdir')
```
把两个路径合成一个时，不要直接拼字符串，而要通过os.path.join()函数，这样可以正确处理不同操作系统的路径分隔符。在Linux/Unix/Mac下，os.path.join()返回这样的字符串：`part-1/part-2`，而Windows下会返回这样的字符串：`part-1\part-2`
同样的道理，要拆分路径时，也不要直接去拆字符串，而要通过os.path.split()函数，这样可以把一个路径拆分为两部分，后一部分总是最后级别的目录或文件名：
```python
os.path.split('/Users/michael/testdir/file.txt')
# ('/Users/michael/testdir', 'file.txt')
```
os.path.splitext()可以直接让你得到文件扩展名
```python
os.path.splitext('/path/to/file.txt')
# ('/path/to/file', '.txt')
```
这些合并、拆分路径的函数并不要求目录和文件要真实存在，它们只对字符串进行操作。
文件操作使用下面的函数。假定当前目录下有一个test.txt文件：
```python
# 对文件重命名:
os.rename('test.txt', 'test.py')
# 删掉文件:
os.remove('test.py')
```
但是复制文件的函数居然在os模块中不存在！原因是复制文件并非由操作系统提供的系统调用。理论上讲，我们通过上一节的读写文件可以完成文件复制，只不过要多写很多代码。
幸运的是shutil模块提供了copyfile()的函数，你还可以在shutil模块中找到很多实用函数，它们可以看做是os模块的补充。
```python
# 列出当前目录下的所有目录
[x for x in os.listdir('.') if os.path.isdir(x)]
# 列出所有的.py文件
[x for x in os.listdir('.') if os.path.isfile(x) and os.path.splitext(x)[1]=='.py']
```