import React from 'react';
import { TextInput as RNTextInput, TextInputProps, View, Text } from 'react-native';
import clsx from 'clsx';
import Ionicons from '@expo/vector-icons/Ionicons';

interface CustomTextInputProps extends TextInputProps {
	placeholder?: string;
	value?: string;
	onChangeText?: (text: string) => void;
	className?: string;
	iconLeft?: string;
	iconRight?: string;
	error?: boolean; // Indica se há um erro
	errorMessage?: string; // Mensagem de erro a ser exibida
	disabled?: boolean;
}

const TextInput = ({
	placeholder,
	value,
	onChangeText,
	className,
	iconLeft,
	iconRight,
	error = false,
	errorMessage,
	style,
	disabled = false,
	...rest
}: CustomTextInputProps) => {
	return (
		<View className="w-full">
			<View
				className={clsx(
					'w-full flex-row items-center h-[40px] bg-white border border-gray-300 rounded dark:bg-gray-800 dark:border-gray-600',
					error && 'border-red-500 dark:border-red-500',
					disabled && 'opacity-50',
					className,
				)}
			>
				{iconLeft && (
					<View className="ml-2">
						<Ionicons name={iconLeft as 'search'} size={20} color={error ? 'red' : 'gray'} />
					</View>
				)}
				<RNTextInput
					autoComplete="off"
					placeholder={placeholder}
					value={value}
					onChangeText={disabled ? undefined : onChangeText}
					className={clsx(
						'flex-1 h-full px-4 text-black dark:text-white placeholder-gray-400 dark:placeholder-gray-500',
						iconLeft && 'ml-2',
						iconRight && 'mr-2',
					)}
					style={style}
					editable={!disabled}
					{...rest}
				/>
				{iconRight && (
					<View className="mr-2">
						<Ionicons name={iconRight as 'search'} size={20} color={error ? 'red' : 'gray'} />
					</View>
				)}
			</View>

			{error && errorMessage && (
				<View className="flex flex-row items-center mt-1">
					<Text className="ml-1 text-sm text-red-500">{errorMessage}</Text>
				</View>
			)}
		</View>
	);
};

export default TextInput;