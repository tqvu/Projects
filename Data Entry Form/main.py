import tkinter
from tkinter import ttk
from tkinter import messagebox
import openpyxl
import os


def enter_data():
    firstname = first_name_entry.get()
    lastname = last_name_entry.get()
    age = age_entry.get()
    email = email_entry.get()
    phone = phone_entry.get()
    comment = comment_entry.get()
    
    if firstname == "" or lastname == "" or age == "" or email == "" or phone == "":
        messagebox.showwarning("Error","Enter required fields")
    
    else:
        filepath = 'C:\\Users\\Tyler\\Projects\\Data Entry Form\\data.xlsx'
        
        if not os.path.exists(filepath):
            workbook = openpyxl.Workbook()
            sheet = workbook.active
            heading = ["First Name", "Last Name", "Age", "Email", "Phone", "Comments"]
            sheet.append(heading)
            workbook.save(filepath)
        
        workbook = openpyxl.load_workbook(filepath)
        sheet = workbook.active
        sheet.append([firstname, lastname, age, email, phone, comment])
        workbook.save(filepath)
    

window = tkinter.Tk()
window.title("Form")
window.geometry("350x450")


frame = tkinter.Frame(window)
frame.pack()

user_info_frame = tkinter.LabelFrame(frame, text= "Enter Data", font= ("Arial", 20))
user_info_frame.grid(padx= 20, pady= (20, 10), ipady= 10, ipadx= 10)


first_name_label = tkinter.Label(user_info_frame, text= "First Name", font= ("Arial", 15))
first_name_label.grid(row= 0)

last_name_label = tkinter.Label(user_info_frame, text= "Last Name", font= ("Arial", 15))
last_name_label.grid(row= 2)

age_label = tkinter.Label(user_info_frame, text= "Age", font= ("Arial", 15))
age_label.grid(row= 4)

email_label = tkinter.Label(user_info_frame, text= "Email Address", font= ("Arial", 15))
email_label.grid(row= 6)

phone_label = tkinter.Label(user_info_frame, text= "Mobile Number", font= ("Arial", 15))
phone_label.grid(row= 8)

comment_label = tkinter.Label(user_info_frame, text= "Comments (optional)", font= ("Arial", 15))
comment_label.grid(row= 10)

first_name_entry = tkinter.Entry(user_info_frame, width= 35, font= ("Arial", 10))
first_name_entry.grid(row= 1)

last_name_entry = tkinter.Entry(user_info_frame, width= 35, font= ("Arial", 10))
last_name_entry.grid(row= 3)

age_entry = tkinter.Spinbox(user_info_frame, from_= 0, to= 1000, width= 34, font= ("Arial", 10))
age_entry.grid(row= 5)

email_entry = tkinter.Entry(user_info_frame, width= 35, font= ("Arial", 10))
email_entry.grid(row= 7)

phone_entry = tkinter.Entry(user_info_frame, width= 35, font= ("Arial", 10))
phone_entry.grid(row= 9)

comment_entry = tkinter.Entry(user_info_frame, width= 35, font= ("Arial", 10))
comment_entry.grid(row= 11)

for widget in user_info_frame.winfo_children():
    widget.grid_configure(padx= 10, sticky="w")
    
button = tkinter.Button(frame, text= "Submit", font= ("Arial", 15), command= enter_data)
button.grid(row= 1, pady= 10, ipadx= 15)

    
    
    


window.mainloop()