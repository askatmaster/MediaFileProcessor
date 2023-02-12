# MediaFileProcessor
C# OpenSource library for processing various files (videos, photos, documents, images).

Данная библиотека является универсальной оболочкой для исполняемых процессов в операционной системе (Windows/Linux).
Библиотека позволяет файлам взаимодействовать с процессами через именованные каналы, потоки, массивы байтов и пути в директориях.
Так же имеет некоторые полезные функции, такие как возможность декодирования потока на лету и получения из него набора файлов по их сигнатурам. 

В данной версии в библиотеки реализованы оболочки над такими проектами как FFmpeg, ImageMagick и Pandoc.
Эту библиотеку так же можно использовать для взаимодействия с сторонними процессами. 

Ниже представления инструкция по использованию данной библиотеки и ее описание. 

После прочтения инструкции вы можете изучить исходный код т.к. он подробно закомментирован и имеет простую архитектуру.

В начале следует определить данные для обработки. Данными для обработки является класс ```MediaFile```. 
Создать экземпляр данного класса можно из потока, пути к файлу, массива байтов, именованного канала, шаблона именования:

```csharp 
var fromPath = new MediaFile(@"C:\fileTest.avi", MediaFileInputType.Path);

var fromNamedPipe = new MediaFile(@"fileTestPipeName", MediaFileInputType.NamedPipe);

var namingTemplate = new MediaFile(@"C:\fileTest%003d.avi", MediaFileInputType.Template);

var fs = @"C:\fileTest.avi".ToStream();
var fromStream = new MediaFile(fs);

var bytes = @"C:\fileTest.avi".ToBytes();
var fromBytes = new MediaFile(bytes);
```
![MediaFileCreate.jpg](ReadmeImages%2FMediaFileCreate.jpg)

При создании экземпляра из пути, именованного канала и шаблона именования необходимо указать тип получения данный через параметр ```MediaFileInputType```.

# Инструкция FFmpeg

Для обработки видеофайлов средствами FFmpeg необходимо иметь его исполняемый файл ffmpeg.exe.
Если вы не хотите скачивать его собственноручно то можете использовать следующий код:

```await VideoFileProcessor.DownloadExecutableFiles();```

Данный код скачает архив по адресу https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip и распокует необходимый ffmpeg.exe в корневую директорию.

## Пример обработки файла

Ниже представлен пример получения кадра из видео.

За обработку видеофайлов средствами ffmpeg отвечает класс ```VideoFileProcessor```.
Следует создать его экземпляр:

```var videoFileProcessor = new VideoFileProcessor();```

Создание через конструктор без параметров подразумевает что исполняемые файлы ffmpeg.exe и ffprobe.exe находятся в корневой папке.

Если вы определили исполняемые файлы в другой директории то создавать экземпляр процессора следует задав пути к исполняемым файлам через конструктор:
```csharp
var videoFileProcessor = new VideoFileProcessor("pathToFFmpeg.exe", "pathToFFprobe.exe");
```

Чтобы указать как следует обрабатывать файл нам необходимо создать экземпляр ```VideoProcessingSettings```.
Далее следует определить конфигурацию для обработкию:
```csharp 
var settings = new VideoProcessingSettings();

var mediaFile = new MediaFile(@"pathToOutputFile", MediaFileInputType.Path);

settings.ReplaceIfExist()                          //Перезаписывать выходные файлы без запроса.
        .Seek(TimeSpan.FromMilliseconds(47500))    //Кадр, с которого нужно начать поиск.
        .SetInputFiles(mediaFile)                  //Установить входные файлы
        .FramesNumber(1)                           //Количество видеокадров для вывода
        .Format(FileFormatType.JPG)                //Форсировать формат входного или выходного файла.
        .SetOutputArguments(@"pathToInputFile");   //Настройка выходных аргументов
```
Далее надо лишь передать конфигурацию в метод  ```ExecuteAsync```:

```csharp 
var result = await videoFileProcessor.ExecuteAsync(settings, new CancellationToken());
```
Указанные методы конфигурации выдадут нам следующие аргументы для запуска процесса ffmpeg:
```-y  -ss 00:00:47.500  -i pathToOutputFile  -frames:v 1  -f image2 pathToInputFile```.
Необходимо СОБЛЮДАТЬ ПОРЯДОК конфигуарций, т.к. некоторые аргументы должны быть заданы до указания входного аргумента и некоторые после. 

### Внимание

При настройке конфигурации процесса вы можете задать входные данные используя метод ```SetInputFiles``` он принимает массив параметров в виде экземпляров класса ```MediaFile```.

Вам следует просто создать экземпляры этого класса из данных представленных в любом виде(путь, поток, байты, каналы, шаблоны) и передать в метод ```SetInputFiles```.
Метод ```SetOutputArguments``` отвечает за установку аргумента выходного файла. Через этот метод можно установить путь выходного файла, адрес rtp сервера для трансляции и т.д.

Если этот метод не вызывать то это значит что результат обработки будет выдан в ```StandardOutput``` в виде потока. И метод ```ExecuteAsync``` вернет результат в потоке.
Если же вы установили свой выходной аргумент то ```StandardOutput``` будет пустой и ```ExecuteAsync``` вернет ```null```.

Если вам нужно установить аргумент которого нету в методах конфигурации то вы можете задать кастомные аргументы с помощью метода ```CustomArguments```.

Полный код:
```csharp
var mediaFile = new MediaFile(@"pathToOutputFile", MediaFileInputType.Path);

var videoFileProcessor = new VideoFileProcessor();

var settings = new VideoProcessingSettings();

settings.ReplaceIfExist()                        //Overwrite output files without asking.
        .Seek(TimeSpan.FromMilliseconds(47500))  //The frame to begin seeking from.
        .SetInputFiles(mediaFile)                //Set input files
        .FramesNumber(1)                         //Number of video frames to output
        .Format(FileFormatType.JPG)              //Force input or output file format.
        .SetOutputArguments(@"pathToInputFile"); //Setting Output Arguments

var result = await videoFileProcessor.ExecuteAsync(settings, new CancellationToken());
```

В текущей версии библиотеки уже реализованы некоторые варианты обработки видеофайлов с помощью ffmpeg:

- Извлечь кадр из видео
- Обрезать видео
- Конвертировать видео в набор изображений покадрово
- Конвертировать изображения в видео
- Извлечь аудиодорожку из видеофайла
- Конвертировать в другой формат
- Добавить Вотермарку
- Удалить звук из видео
- Добавить аудиофайл в видеофайл
- Конвертировать видео в Gif анимацию
- Сжать видео
- Сжать изображение
- Соединить набор видеофайлов в единый видеофайл
- Добавить субтитры
- Получить подробную информацию по метаданныхм видеофайла

### Пример "Извлечь кадр из видео"
Ниже представлен пример применения извлечения кадра из видеофайла на определенном тайминге при условии что файл существует ФИЗИЧЕСКИ в директории 
```csharp
 var videoFileProcessor = new VideoFileProcessor();
 //Test block with physical paths to input and output files
 await videoFileProcessor.GetFrameFromVideoAsync(TimeSpan.FromMilliseconds(47500),
                                                 new MediaFile(@"C:\inputFile.avi", MediaFileInputType.Path),
                                                 @"C:\resultPath.jpg",
                                                 FileFormatType.JPG);
```

Ниже представлен пример применения извлечения кадра из видеофайла на определенном тайминге при условии если у нас файл в видео массива байтов
```csharp
//Block for testing file processing as bytes without specifying physical paths
 var bytes = await File.ReadAllBytesAsync(@"C:\inputFile.avi");
 var resultBytes = await videoProcessor.GetFrameFromVideoAsBytesAsync(TimeSpan.FromMilliseconds(47500), new MediaFile(bytes), FileFormatType.JPG);
 await using (var output = new FileStream(@"C:\resultPath.jpg", FileMode.Create))
     output.Write(resultBytes);
```

Ниже представлен пример применения извлечения кадра из видеофайла на определенном тайминге при условии если у нас файл в видео потока
```csharp
//Block for testing file processing as streams without specifying physical paths
await using var stream = new FileStream(@"C:\inputFile.avi", FileMode.Open);
var resultStream = await videoProcessor.GetFrameFromVideoAsStreamAsync(TimeSpan.FromMilliseconds(47500), new MediaFile(stream), FileFormatType.JPG);
await using (var output = new FileStream(@"C:\resultPath.jpg", FileMode.Create))
     resultStream.WriteTo(output);
```

Все остальные методы работают точно также. Вы можете передать файлы в процесс в любом виде и получить в любом видео.

# Инструкция ImageMagick

Для обработки изображений применяется ImageMagick его класс ```ImageFileProcessor``` и его исполняемый файл convert.exe

Для загрузки его исполняемого файла можете вызвать следующий код
```csharp
await ImageFileProcessor.DownloadExecutableFiles();
```
Данный код скачат исполняемый файл в корневую директорию с адреса https://imagemagick.org/archive/binaries/ImageMagick-7.1.0-61-portable-Q16-x64.zip

Вся инструкция которая относилась к ffmpeg, так же относится и к ImageMagick.
Обработчиком ImageMagick является класс ```ImageFileProcessor```
```csharp
var i = new ImageFileProcessor();
var j = new ImageFileProcessor("pathToConvert.exe");
```

В текущей версии библиотеки уже реализованы некоторые варианты обработки изображений с помощью ImageMagick:

-Сжать изображение
-Конвертировать изображение в другой формат
-Изменить размер изображения
-Преобразовать набор изображений в Gif анимацию

### Пример сжатия изображения в трех вариантах (путь в директории, поток, массив байтов)
```csharp
//Test block with physical paths to input and output files
await processor.CompressImageAsync(new MediaFile(_image, MediaFileInputType.Path), ImageFormat.JPG, 60, FilterType.Lanczos, "x1080", @"С:\result.jpg", ImageFormat.JPG);

//Block for testing file processing as streams without specifying physical paths
await using var stream = new FileStream(_image, FileMode.Open);
var resultStream = await processor.CompressImageAsStreamAsync(new MediaFile(stream), ImageFormat.JPG, 60, FilterType.Lanczos, "x1080", ImageFormat.JPG);
await using (var output = new FileStream(@"С:\result.jpg", FileMode.Create))
     resultStream.WriteTo(output);

//Block for testing file processing as bytes without specifying physical paths
var bytes = await File.ReadAllBytesAsync(_image);
var resultBytes = await processor.CompressImageAsBytesAsync(new MediaFile(bytes), ImageFormat.JPG, 60, FilterType.Lanczos, "x1080", ImageFormat.JPG);
await using (var output = new FileStream(@"С:\result.jpg", FileMode.Create))
    output.Write(resultBytes);
```

# Инструкция Pandoc
Для обработки документов применяется процесс pandoc.exe, его процессор ```DocumentFileProcessor```.

В текущей версии библиотеки уже реализованы некоторые варианты обработки документов с помощью pandoc:

-конвертирование файла .docx в .pdf
```csharp
var file = new MediaFile(@"C:\inputFile.docx", MediaFileInputType.Path);
var processor = new DocumentFileProcessor();
await processor.ConvertDocxToPdf(file, "test.pdf");
```

# Полезные функции

## MultiStream
Класс ```MultiStream``` предназначен для работы с набором потоков как с единым целлым.

Если вам нужно передать множество файлов в единый входной поток процесса, то вам поможет класс ```MultiStream```.
К примеру вариант когда ffmpeg должен создать видео из набора изображений, и эти изображения следует передать единым потоком в входной поток процесса.
```csharp
var stream = new MultiStream();
stream.AddStream(new FileStream(@"С:\inputfile1.jpg", FileMode.Open, FileAccess.Read, FileShare.Read));
stream.AddStream(new FileStream(@"С:\inputfile2.jpg", FileMode.Open, FileAccess.Read, FileShare.Read));
stream.AddStream(new FileStream(@"С:\inputfile3.jpg", FileMode.Open, FileAccess.Read, FileShare.Read));
stream.AddStream(new FileStream(@"С:\inputfile4.jpg", FileMode.Open, FileAccess.Read, FileShare.Read));
stream.AddStream(new FileStream(@"С:\inputfile5.jpg", FileMode.Open, FileAccess.Read, FileShare.Read));
```
Здесь мы создаем экземпляр класса ```MultiStream``` и через метод ```AddStream``` добавляем в этот потом несколько потоков с различными файлами.
Теперь мы может эти потоки передать в процесс одним потоком в один входной поток

### Пример использоваения MultiStream
```csharp
var stream = new MultiStream();
var files = new List<string>();
for (var i = 1; i <= 1000; i++)
{
    files.Add($@"C:\image{i:000}.jpg");
}
foreach (var file in files)
{
    stream.AddStream(new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read));
}

//Block for testing file processing as streams without specifying physical paths
stream.Seek(0, SeekOrigin.Begin);
var resultStream = await videoProcessor.ConvertImagesToVideoAsStreamAsync(new MediaFile(stream), 24, "yuv420p", FileFormatType.AVI);
await using (var output = new FileStream(@"C:\mfptest\results\ConvertImagesToVideoTest\resultStream.avi", FileMode.Create))
{
   resultStream.WriteTo(output);
}
```
Собираем тысячу изображений в один ```MultiStream``` и передаем в процесс
У класса ```MultiStream``` есть метод ```ReadAsDataArray``` чтобы получить содержащиеся потоки в виде массивов байтов,
и ```ReadAsStreamArray``` чтобы получить содержащиеся потоки в виде массива потоков.

## Декодирование потока на лету
Когда мы используем функцию ffmpeg по разбиению видеофайла покадрово на изображения то он создает нам в указанной выходной директорию набор изображений. 

Но что если нам надо получить его результат на в директорию а в выходной поток. В таком случае он в единый выходной поток запищет все изображения полученные из видеофайла.
В результате у нас в одном потоке будет множество файлов. Как нам получить эту файлы?
Тут на помощь приходит метод расширения ```GetMultiStreamBySignature(this Stream stream, byte[] fileSignature)```.
Этот следует вызвать на потоке который следует декодировать и передать в этот метод в качестве аргумента - сигнатуру извлекаемых файлов.
Результатом этого метода будет ```MultiStream``` содержащий в себе массив поков файлов. 1 поток для 1 файла. 
И уже используя его методы ```ReadAsDataArray``` или ```ReadAsStreamArray``` мы можем получить эти файлы в виде массива байтов или потоков. 

### Чтобы подробнее изучить процесс декодирования я советую изучить исходный код.
Наглядный пример декодирования потока:
```csharp
//Block for testing file processing as streams without specifying physical paths
await using var stream = new FileStream(_video1, FileMode.Open);
var resultMultiStream = await videoProcessor.ConvertVideoToImagesAsStreamAsync(new MediaFile(stream), FileFormatType.JPG);
var count = 1;
var data = resultMultiStream.ReadAsDataArray();

foreach (var bytes in data)
{
   await using (var output = new FileStream(@$"C:\result{count++}.jpg", FileMode.Create))
       output.Write(bytes, 0, bytes.Length);
}
```
Для поулчения сигнатуры определенного формата файла есть метод расширения
```csharp
public static byte[] GetSignature(this FileFormatType outputFormatType)
```

Если данный метод расширения не поддерживает определение сигнатуры нужного вам формата то дайте мне знать и я максимально быстро исправлю недочет. 

## FileDownloadProcessor

Если вам необходимо скачать файл то можете использовать статичный метод ```DownloadFile``` класса ```FileDownloadProcessor```.
Этот метод использует для скачивания не устаревщий WebClient а HttpClient и позволяет в процентах отслеживать прогресс скачивания. 

## ZipFileProcessor

Для работы с zip архивами представлен класс ```ZipFileProcessor```.

Применения для распаковки скачанного архива ffmpeg и извлечение исполняемых файлов
```csharp
// Open an existing zip file for reading
            using(var zip = ZipFileProcessor.Open(fileName, FileAccess.Read))
            {
                // Read the central directory collection
                var dir = zip.ReadCentralDir();

                // Look for the desired file
                foreach (var entry in dir)
                {
                    if (Path.GetFileName(entry.FilenameInZip) == "ffmpeg.exe")
                    {
                        zip.ExtractFile(entry, $@"ffmpeg.exe"); // File found, extract it
                    }

                    if (Path.GetFileName(entry.FilenameInZip) == "ffmpeg.exe")
                    {
                        zip.ExtractFile(entry, $@"ffprobe.exe"); // File found, extract it
                    }
                }
            }
```
# MediaFileProcess

Пожалуй главным классом этой бибилиотеки является класс ```MediaFileProcess```.
Он является универсальной оболочкой для исполняемых процессов.

При создании его экземпляра следует задать ему путь/имя исполняемого процесса, аргументы процесса, ```ProcessingSettings```, входные потоки и наименования входных именованных каналов.
### Примечание по входным потокам и именованным каналам:
Если в процесс необходимо передать множество потоков в разные входные аргументы, 
то в входных аргументам следует указать наименования именованных каналов и передать эти имена и входные потоки в соответствующие аргументы конструктора ```MediaFileProcess```.
Это необходимо т.к. в случае передачи разным потоков в разные входные аргументы применяются именованные каналы.
Настройку самого исполняемого процесса необходимо выполнить в классе ```ProcessingSettings```.