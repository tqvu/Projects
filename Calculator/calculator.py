from tkinter import *

DIMGRAY = "#696969"

def button_press(num):
    global equation_string
    equation_string = equation_string + str(num)
    equation_label.set(equation_string)

def equals():
    global equation_string
    
    try:
        total = str(eval(equation_string))
        equation_label.set(total)
        
        equation_string = total
        
    except SyntaxError:
        equation_label.set("Error")
        equation_string = "Error"
        
    
    except ZeroDivisionError:
        equation_label.set("Error")
        equation_string = "Error"

def clear():
    global equation_string
    equation_string = ""
    equation_label.set(equation_string)

window = Tk()
window.title("Calculator")
window.geometry("500x630")
window.configure(bg="black")

equation_string = ""
equation_label = StringVar()

label = Label(window, textvariable= equation_label, bg= DIMGRAY, width= 13, height= 1, font= ("Arial", 35), pady= 0)
label.pack(pady= 15, padx= 10)

frame = Frame(window)
frame.pack()

btns = []
btns_nmbr = 1

for x in range(0, 3):
    for y in range(0, 3):
        btns.append(Button(frame, text= btns_nmbr, height= 4, width= 9, font= 35, command= lambda btns_nmbr= btns_nmbr: button_press(btns_nmbr)))
        btns[btns_nmbr - 1].grid(row=x, column=y)
        btns_nmbr += 1

button0 = Button(frame, text= 0, height= 4, width= 9, font= 35, command= lambda: button_press(0))
button0.grid(row= 3, column= 0)

plus = Button(frame, text= "+", height= 4, width= 9, font= 35, command= lambda: button_press("+"))
plus.grid(row= 0, column= 3)

minus = Button(frame, text= "-", height= 4, width= 9, font= 35, command= lambda: button_press("-"))
minus.grid(row= 1, column= 3)

multiply = Button(frame, text= "*", height= 4, width= 9, font= 35, command= lambda: button_press("*"))
multiply.grid(row= 2, column= 3)

divide = Button(frame, text= "/", height= 4, width= 9, font= 35, command= lambda: button_press("/"))
divide.grid(row= 3, column= 3)

equal = Button(frame, text= "=", height= 4, width= 9, font= 35, command= equals)
equal.grid(row= 3, column= 2)

decimal = Button(frame, text= ".", height= 4, width= 9, font= 35, command= lambda: button_press("."))
decimal.grid(row= 3, column= 1)

clear = Button(window, text= "clear", height= 4, width= 19, font= 35, command= clear)
clear.pack()

window.mainloop()