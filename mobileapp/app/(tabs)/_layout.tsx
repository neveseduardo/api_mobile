import { Tabs } from 'expo-router';
import React from 'react';
import { Platform } from 'react-native';

import { HapticTab } from '@/components/HapticTab';
import TabBarBackground from '@/components/ui/TabBarBackground';
import { Colors } from '@/constants/Colors';
import { useColorScheme } from '@/hooks/useColorScheme';
import "@/styles/global.css";
import Ionicons from '@expo/vector-icons/Ionicons';

export default function TabLayout() {
	const colorScheme = useColorScheme();

	return (
		<Tabs
			screenOptions={{
				tabBarActiveTintColor: Colors[colorScheme ?? 'light'].tint,
				headerShown: false,
				tabBarButton: HapticTab,
				tabBarBackground: TabBarBackground,
				tabBarStyle: Platform.select({
					ios: {
						// Use a transparent background on iOS to show the blur effect
						position: 'absolute',
					},
					default: {},
				}),
			}}
		>
			<Tabs.Screen
				name="index"
				options={{
					title: 'Home',
					tabBarIcon: ({ color }) => <Ionicons name="home" size={25} color={color} />,
				}}
			/>
			<Tabs.Screen
				name="products"
				options={{
					title: 'Produtos',
					tabBarIcon: ({ color }) => <Ionicons name="list" size={25} color={color} />,
				}}
			/>
			<Tabs.Screen
				name="user"
				options={{
					title: 'Dados pessoais',
					tabBarIcon: ({ color }) => <Ionicons name="person" size={25} color={color} />,
				}}
			/>
		</Tabs>
	);
}
