import create from "zustand";
import { devtools } from "zustand/middleware";
import {UserController} from "@/UserModule/Controller/UserController";

const useUserStore = create(
    devtools((set) => ({
        // calling a function to initialize the state feels wrong, search for it.
        user: UserController.GetLocalUser(),
        setUser: (user: any) => set((state: any) => ({ ...state, user: user })),
        removeUser: () => set({ user: {} }),
    }))
);

export { useUserStore };
